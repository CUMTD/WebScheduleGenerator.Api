using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlType("TRIP_CONSTRAINTS_TEXT")]
public class InitFootnote
{

	[XmlElement("SYMBOL")]
	public required string Symbol { get; set; }

	[XmlElement("TEXT")]
	public required string Content { get; set; }
}
