namespace WebScheduleGenerator.Core.Entities.Schedule
{
	public class Stop
	{
		public required string Id { get; set; }
		public required string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public required string SmsCode { get; set; }
		public required string TimePointLetter { get; set; }
	}
}
