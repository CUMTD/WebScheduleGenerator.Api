using System.Collections;

namespace WebScheduleGenerator.Core.Entities.Schedule
{
	public class ProcessingResult : IReadOnlyCollection<Timetable>
	{

		private readonly Timetable[] _timetables;
		public int Count => _timetables.Length;

		public ProcessingResult(IEnumerable<Timetable> timetables)
		{
			_timetables = timetables?.ToArray() ?? throw new ArgumentNullException(nameof(timetables));
		}

		public IEnumerator<Timetable> GetEnumerator() =>
			((IEnumerable<Timetable>)_timetables).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

	}
}
