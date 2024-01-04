namespace WebScheduleGenerator.Core.Entities.Stopwatch
{
	public abstract class Stop
	{
		public string Id { get; set; }
		public string? SmsCode { get; private set; }

		private string _name;
		public string Name
		{
			get => _name;
			set => _name = value?.Replace(" and ", " & ") ?? string.Empty;
		}
		public virtual string? City { get; set; }

		public string? Description { get; set; }
		public double Latitude { get; protected set; }
		public double Longitude { get; protected set; }
		public string? Url { get; set; }
		public string? Timezone { get; set; }
		public bool Accessible { get; set; }
		public bool Active { get; set; }
		public DateTime ImportTime { get; set; }

		protected Stop()
		{
			Id = string.Empty;
			_name = string.Empty;
		}

		protected Stop(string id, string name, double latitude, double longitude, string city, DateTime importTime, string timezone = "America/Chicago", bool accessible = true, bool active = true) : this()
		{
			Id = id;
			Name = name;
			City = city;
			Timezone = timezone;
			Accessible = accessible;
			Active = active;
			ImportTime = importTime;
			Latitude = latitude;
			Longitude = longitude;
		}
	}
}
