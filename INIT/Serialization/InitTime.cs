using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("SJTIME")]
	public class InitTime
	{

		[XmlArray("SJTFNS")]
		[XmlArrayItem("SJTFN")]
		public string[]? Footnotes { get; set; }

		[XmlText]
		public string? Text { get; set; }

		[XmlAttribute("REH")]
		public bool REH { get; set; }

		[XmlAttribute("BKCOL")]
		public string? BackgroundColor { get; set; }

		[XmlAttribute("FONTCOL")]
		public string? ForegroundColor { get; set; }

		[XmlIgnore]
		public bool IsHopper =>
			!(string.IsNullOrEmpty(ForegroundColor) && string.IsNullOrEmpty(BackgroundColor));

		[XmlIgnore]
		public bool REHSpecified { get; set; }
	}
}



