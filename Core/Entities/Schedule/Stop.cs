namespace WebScheduleGenerator.Core.Entities.Schedule
{
	public class Stop
	{
		public required string Id { get; set; }
		public required string Name { get; set; }
		public string? Parentheses { get; set; }
		public required string SmsCode { get; set; }
		public required string TimePointLetter { get; set; }
	}
}
