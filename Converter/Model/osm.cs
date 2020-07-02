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

        private osmNodeTag[] tagField;

        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public osmNodeTag[] tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }

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

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class osmNodeTag
    {

        private string kField;

        private string vField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string k
        {
            get
            {
                return this.kField;
            }
            set
            {
                this.kField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string v
        {
            get
            {
                return this.vField;
            }
            set
            {
                this.vField = value;
            }
        }
    }
}
