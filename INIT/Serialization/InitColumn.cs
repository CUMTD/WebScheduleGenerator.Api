using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("COLUMN")]
	public class InitColumn
	{

		[XmlAttribute("idx")]
		public int Index { get; set; }

		[XmlAttribute("w")]
		public int Width { get; set; }

		[XmlAttribute("eor")]
		public bool HasEndOfRoute { get; set; }

		[XmlIgnore]
		public bool HasEndOfRouteSpecified { get; set; }
	}
}
