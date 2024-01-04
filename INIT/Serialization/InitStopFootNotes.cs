using System.Xml.Serialization;

namespace WebScheduleGenerator.Init.Serialization
{
	[Serializable, XmlType("SJTFNS")]
	public class InitStopFootNotes
	{
		[XmlElement("SJTFN")]
		public string[] Letters { get; set; }

		public InitStopFootNotes()
		{
			Letters = [];
		}
	}
}
