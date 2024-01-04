using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("STOP")]
	public class InitStop
	{

		[XmlAttribute("stopnumber")]
		public int InitId { get; set; }

		[XmlAttribute("sms")]
		public required string SmsCode { get; set; }

		[XmlAttribute("tpID")]
		public required string TimePointLetter { get; set; }

		[XmlAttribute("stopshortname")]
		public required string Id { get; set; }

		[XmlText]
		public required string Name { get; set; }
	}
}
