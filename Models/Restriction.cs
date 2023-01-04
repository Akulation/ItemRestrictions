using System.Xml.Serialization;

namespace ItemRestrictions.Models
{
    public class Restriction
    {
        public string PickupPermission { get; set; }

        [XmlArrayItem("itemID")]

        public ushort[] Items { get; set; }
    }
}
