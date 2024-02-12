using System.Collections;

namespace WebScheduleGenerator.Core.Entities.Schedule;

public class ProcessingResult(IEnumerable<Timetable> timetables) : IReadOnlyCollection<Timetable>
{

	private readonly Timetable[] _timetables = timetables?.ToArray() ?? throw new ArgumentNullException(nameof(timetables));
	public int Count => _timetables.Length;

	public IEnumerator<Timetable> GetEnumerator() =>
		((IEnumerable<Timetable>)_timetables).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
