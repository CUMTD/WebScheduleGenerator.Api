using WebScheduleGenerator.Core.Entities.Schedule;

namespace WebScheduleGenerator.Core;

public interface IScheduleConverter<T> where T : class
{
	Task<ProcessingResult> ConvertScheduleAsync(T schedule, CancellationToken cancellationToken);
	Task<ProcessingResult> ConvertScheduleAsync(Stream schedule, CancellationToken cancellationToken);
}
