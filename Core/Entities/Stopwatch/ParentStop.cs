namespace WebScheduleGenerator.Core.Entities.Stopwatch
{
	public class ParentStop : Stop
	{
		public bool? IsIStop { get; set; }

		public bool IsStation { get; set; }

		public bool IsTimepoint { get; set; }

		protected ParentStop()
		{
		}

		public ParentStop(
			string id,
			string name,
			string city,
			double latitude,
			double longitude,
			DateTime importTime,
			string timezone = "America/Chicago",
			bool accessible = true,
			bool active = true
		) : base(id, name, latitude, longitude, city, importTime, timezone, accessible, active)
		{
		}
	}
}
