using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("TRIP_CONSTRAINTS_TEXTS")]
	public class InitAllFootnotes
	{
		[XmlElement("TRIP_CONSTRAINTS_TEXT")]
		public InitFootnote[]? Footnotes { get; set; }

		[XmlAttribute("REHset")]
		public bool REHset { get; set; }
	}
}
