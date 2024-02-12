using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlType(TypeName = "ROUTE")]
public class InitRoute
{
	[XmlElement("ROUTE_NUMBER")]
	public required InitRouteInfo RouteInfo { get; set; }

	[XmlElement("VALIDITY")]
	public required string Validity { get; set; }

	[XmlElement("DIRECTION")]
	public required InitDirection[] Directions { get; set; }

	[XmlAttribute("split_directions")]
	public bool ShouldSplitDirections { get; set; }
}
