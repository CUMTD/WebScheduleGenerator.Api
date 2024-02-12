using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlType("ROUTE_NUMBER")]
public class InitRouteInfo
{
	[XmlAttribute("color")]
	public required string HexColor { get; set; }

	[XmlAttribute("fontcol")]
	public required string HexTextColor { get; set; }

	[XmlText]
	public required string Name { get; set; }
}
