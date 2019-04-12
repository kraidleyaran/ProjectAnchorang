using System.Xml.Serialization;

namespace CreativeSpore.TiledImporter
{
    [XmlRoot("TileProperty")]
    public class TmxTileProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
