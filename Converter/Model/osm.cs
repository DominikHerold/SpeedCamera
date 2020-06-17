using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Model
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class osm
    {
        private osmNode[] nodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("node")]
        public osmNode[] node
        {
            get
            {
                return this.nodeField;
            }
            set
            {
                this.nodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class osmNode
    {
        private double latField;

        private double lonField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double lat
        {
            get
            {
                return this.latField;
            }
            set
            {
                this.latField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double lon
        {
            get
            {
                return this.lonField;
            }
            set
            {
                this.lonField = value;
            }
        }
    }


}
