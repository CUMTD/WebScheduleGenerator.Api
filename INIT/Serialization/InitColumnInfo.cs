using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("COLUMNINFO")]
	public class InitColumnInfo
	{

		[XmlElement("COLUMN")]
		public required InitColumn[] Columns { get; set; }

		[XmlAttribute("sum")]
		public int TotalWidth { get; set; }

		[XmlAttribute("eor")]
		public int ColumnsWithEndOfRoute { get; set; }
	}
}
