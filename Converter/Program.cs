using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Converter.Model;
using Newtonsoft.Json;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var jsonAreas = new List<Area>();
            var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            var data = HttpUtility.UrlEncode(
                @"node
  [highway=speed_camera]
  (44.29240108529005,3.44970703125,57.100452089370705,17.4462890625);
out;");
            var toSend = $"data={data}";
            var responseMessage = client.PostAsync("https://overpass-api.de/api/interpreter", new StringContent(toSend, Encoding.UTF8, "application/x-www-form-urlencoded")).GetAwaiter().GetResult();

            using (var content = responseMessage.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
            {
                var osmData = (osm)new XmlSerializer(typeof(osm)).Deserialize(content);

                foreach (var node in osmData.node)
                {
                    var lat = node.lat;
                    var lon = node.lon;
                    var maxSpeed = node.tag.FirstOrDefault(tag => tag.k.Equals("maxspeed", StringComparison.OrdinalIgnoreCase))?.v;
                    if (!string.IsNullOrEmpty(maxSpeed) && maxSpeed.Any(character => !char.IsDigit(character)))
                    {
                        maxSpeed = null;
                    }

                    var nw = CalculateProjectWaypoint(lat, lon, 0.25, 315);
                    var se = CalculateProjectWaypoint(lat, lon, 0.25, 135);

                    jsonAreas.Add(
                        new Area
                        {
                            latN = nw.Item1,
                            latS = se.Item1,
                            longE = se.Item2,
                            longW = nw.Item2,
                            MaxSpeed = maxSpeed
                        });
                }
            }

            var result = JsonConvert.SerializeObject(jsonAreas.ToArray(), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(@"C:\Projects\SpeedCamera\docs\areas.json", result);

            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static Tuple<double, double> CalculateProjectWaypoint(double lat, double lon, double distance, double angle)
        {
            var c = distance / (6378137.0 / 1000.0);
            double a;
            double g;

            if (lat >= 0)
            {
                a = (90.0 - lat) * Math.PI / 180.0;
            }
            else
            {
                a = lat * Math.PI / 180.0;
            }

            var q = (360.0 - angle) * Math.PI / 180;
            var b = Math.Acos(Math.Cos(q) * Math.Sin(a) * Math.Sin(c) + Math.Cos(a) * Math.Cos(c));
            var latZiel = 90 - (b * 180 / Math.PI);
            if (latZiel > 90)
            { //Suedhalbkugel - 180 Grad abziehen
                latZiel -= 180;
            }
            if ((a == 0) || (b == 0))
            {
                g = 0;
            }
            else
            {
                var arg = (Math.Cos(c) - Math.Cos(a) * Math.Cos(b)) / (Math.Sin(a) * Math.Sin(b));
                if (arg <= -1)
                {
                    g = Math.PI;
                }
                else if (arg < 1)
                {
                    g = Math.Acos(arg);
                }
                else
                {
                    g = 0;
                }
            }

            if (angle <= 180)
            {
                g = -1 * g;
            }
            var lonZiel = (lon - g * 180 / Math.PI);

            return new Tuple<double, double>(Math.Round(latZiel, 4, MidpointRounding.ToEven), Math.Round(lonZiel, 4, MidpointRounding.ToEven));
        }
    }
}
