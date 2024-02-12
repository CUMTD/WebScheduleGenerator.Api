using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization;

[Serializable, XmlRoot(ElementName = "TIMETABLE", Namespace = "", IsNullable = false)]
public class InitTimetable
{
	[XmlElement("ROUTE")]
	public required InitRoute Route { get; set; }
}

