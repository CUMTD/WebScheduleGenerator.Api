using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlType("Direction")]
public class InitDirection
{
	[XmlAttribute("dirextion")]
	public int Id { get; set; }

	[XmlElement("LINEBAND")]
	public required string LongDirection { get; set; }

	[XmlElement("DIRECTION_TEXT")]
	public required string ShortDirection { get; set; }

	[XmlElement("TYPE_OF_DAY")]
	public required InitDaytype Daytype { get; set; }

}
