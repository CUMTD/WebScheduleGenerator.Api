using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("TYPE_OF_DAY")]
	public class InitDaytype
	{
		[XmlElement("COLUMNINFO")]
		public required InitColumnInfo ColumnInfo { get; set; }

		[XmlArray("STOPS")]
		[XmlArrayItem("STOP", IsNullable = false)]
		public required InitStop[] Stops { get; set; }

		[XmlArray("SMSCODE")]
		[XmlArrayItem("SMS", IsNullable = false)]
		public required string[] SmsCodes { get; set; }

		[XmlArray("SERVICE_JOURNEYS")]
		[XmlArrayItem("SERVICE_JOURNEY", IsNullable = false)]
		public required InitServiceJourney[] ServiceJournies { get; set; }

		[XmlElement("TRIP_CONSTRAINTS_TEXTS")]
		public required InitAllFootnotes AllFootnotes { get; set; }

		[XmlAttribute("typeofday")]
		public byte TypeOfDayDistinguisher { get; set; }
	}
}
