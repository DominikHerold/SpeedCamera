using System;
using System.Collections.Generic;
using System.IO;
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

            using (var stream = new FileStream(@"C:\Projects\SpeedCamera\OverpassTurboRaw.xml", FileMode.Open))
            {
                var osmData = (osm)new XmlSerializer(typeof(osm)).Deserialize(stream);

                foreach (var node in osmData.node)
                {
                    var lat = node.lat;
                    var lon = node.lon;

                    var nw = CalculateProjectWaypoint(lat, lon, 0.2, 315);
                    var se = CalculateProjectWaypoint(lat, lon, 0.2, 135);

                    jsonAreas.Add(new Area { latN = nw.Item1, latS = se.Item1, longE = se.Item2, longW = nw.Item2 });
                }
            }

            var result = JsonConvert.SerializeObject(jsonAreas.ToArray());

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

            return new Tuple<double, double>(latZiel, lonZiel);
        }
    }
}
