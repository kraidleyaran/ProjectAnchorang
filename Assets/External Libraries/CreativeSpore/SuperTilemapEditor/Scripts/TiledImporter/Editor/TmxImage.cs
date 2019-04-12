using System.Xml.Serialization;

namespace CreativeSpore.TiledImporter
{
    [XmlRoot("Image")]
    public class TmxImage
    {
        [XmlAttribute("source")]
        public string Source { get;set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }
        
    }
}
