using System;
using System.Xml.Serialization;

namespace FollowMe.Configuration
{
    [Serializable]
    public class TrackingConfig
    {
        [XmlElement]
        public DateTime Date { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public int SearchObjectSizePixels { get; set; }

        [XmlElement]
        public int HueMin { get; set; }

        [XmlElement]
        public int HueMax { get; set; }

        [XmlElement]
        public float SaturationMin { get; set; }

        [XmlElement]
        public float SaturationMax { get; set; }

        [XmlElement]
        public float LuminanceMin { get; set; }

        [XmlElement]
        public float LuminanceMax { get; set; }
    }
}