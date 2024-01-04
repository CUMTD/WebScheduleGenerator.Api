namespace WebScheduleGenerator.Core.Entities.Schedule
{
	public class Row
	{
		public List<Time> Times { get; set; }
		public Row()
		{
			Times = [];
		}
	}
}
