using System.Collections.Immutable;

namespace WebScheduleGenerator.Core.Entities.Schedule
{
	public class Row
	{
		public ImmutableArray<Time> Times { get; protected set; }
		public Row()
		{
		}

		public static Row CreateEmpty() => new () { Times = [] };
		public static Row Create(IEnumerable<Time> times) => new() { Times = [.. times] };
	}
}
