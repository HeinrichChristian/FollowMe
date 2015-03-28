﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowMe.Configuration
{
    [Serializable()]
    public class TrackingConfig
    {
          [System.Xml.Serialization.XmlElement()]
        public DateTime Date { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public string Name { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public int SearchObjectSizePixels { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public int HueMin { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public int HueMax { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public float SaturationMin { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public float SaturationMax { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public float LuminanceMin { get; set; }

          [System.Xml.Serialization.XmlElement()]
        public float LuminanceMax { get; set; }
    }
}
