using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlType("SERVICE_JOURNEY")]
public class InitServiceJourney
{
	[XmlElement("SJTIME")]
	public required InitTime[] Times { get; set; }

	[XmlAttribute("index")]
	public int Index { get; set; }

	[XmlAttribute("empty")]
	public bool IsEmpty { get; set; }

	[XmlIgnore]
	public bool IsEmptySpecified { get; set; }
}
