using System.Xml.Serialization;

namespace Run00.NuProductCecil
{
	[XmlRoot("product")]
	public class Product
	{
		[XmlElement(ElementName = "copyversionfrom")]
		public string CopyVersionFrom { get; set; }
	}
}
