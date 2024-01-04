using System.Xml.Serialization;
using WebScheduleGenerator.Init.Serialization;

namespace WebScheduleGenerator.Init
{
	public class InitXmlParser
	{
		private static readonly XmlSerializer Serializer = new(typeof(InitTimetable));

		public static InitTimetable Deserialize(Stream stream) =>
			Serializer.Deserialize(stream) is not InitTimetable timetable ?
			throw new Exception("Failed to deserialize") :
			timetable;

		public static InitTimetable Deserialize(string xml)
		{
			using var textReader = new StringReader(xml);
			return Serializer.Deserialize(textReader) is not InitTimetable timetable ?
				throw new Exception("Failed to deserialize") :
				timetable;
		}

	}
}
