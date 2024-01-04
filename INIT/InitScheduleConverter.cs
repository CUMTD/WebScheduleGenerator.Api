using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebScheduleGenerator.Core;
using WebScheduleGenerator.Core.Entities.Schedule;
using WebScheduleGenerator.Core.Entities.Stopwatch;
using WebScheduleGenerator.EF;
using WebScheduleGenerator.Init.Serialization;

namespace WebScheduleGenerator.Init
{
	public class InitScheduleConverter(StopwatchContext stopwatchContext, ILogger<InitScheduleConverter> logger) : IScheduleConverter<InitTimetable>
	{
		public async Task<ProcessingResult> ConvertScheduleAsync(InitTimetable schedule, CancellationToken cancellationToken)
		{
			var timetables = new List<Timetable>();
			foreach (var direction in schedule.Route.Directions)
			{
				var converted = await DirectionToTimetable(schedule, direction, cancellationToken)
					.ConfigureAwait(false);
				timetables.Add(converted);
			}

			return new ProcessingResult(timetables);
		}

		public Task<ProcessingResult> ConvertScheduleAsync(string schedule, CancellationToken cancellationToken)
		{
			var timetable = InitXmlParser.Deserialize(schedule);
			return ConvertScheduleAsync(timetable, cancellationToken);
		}
		public Task<ProcessingResult> ConvertScheduleAsync(Stream schedule, CancellationToken cancellationToken)
		{
			var timetable = InitXmlParser.Deserialize(schedule);
			return ConvertScheduleAsync(timetable, cancellationToken);
		}

		private async Task<Timetable> DirectionToTimetable(InitTimetable timetable, InitDirection direction, CancellationToken cancellationToken)
		{
			var stops = await DirectionToStops(direction, cancellationToken)
				.ConfigureAwait(false);

			return new Timetable()
			{
				Route = RouteInfoToRoute(timetable.Route),
				Direction = direction.ShortDirection,
				Stops = stops,
				Rows = DirectionToRows(direction),
				Footnotes = DirectionToFootnotes(direction)
			};
		}

		private async Task<Core.Entities.Schedule.Stop[]> DirectionToStops(InitDirection direction, CancellationToken cancellationToken)
		{
			var stops = new List<Core.Entities.Schedule.Stop>();
			foreach (var stop in direction.Daytype.Stops)
			{
				var converted = await StopToEntityStop(stop, cancellationToken)
					.ConfigureAwait(false);

				stops.Add(converted);
			}
			return [.. stops];
		}

		private async Task<Core.Entities.Schedule.Stop> StopToEntityStop(InitStop stop, CancellationToken cancellationToken)
		{
			var dbStop = await GetStopByIdOrDefaultAsync(stop.Id, cancellationToken)
				.ConfigureAwait(false);

			return new Core.Entities.Schedule.Stop
			{
				Id = stop.Id,
				Name = dbStop?.Name ?? stop.Name,
				SmsCode = dbStop?.SmsCode ?? stop.SmsCode,
				Latitude = dbStop?.Latitude ?? -1d,
				Longitude = dbStop?.Longitude ?? -1d,
				TimePointLetter = stop.TimePointLetter
			};
		}

		private static Route RouteInfoToRoute(InitRoute route) => new Route
		{
			HexColor = route.RouteInfo.HexColor,
			HexTextColor = route.RouteInfo.HexTextColor,
			Name = route.RouteInfo.Name
		};

		private static Row[] DirectionToRows(InitDirection direction)
		{
			var columns = new List<Row>();

			foreach (var sj in direction.Daytype.ServiceJournies)
			{
				var column = new Row();
				foreach (var sjt in sj.Times)
				{
					var time = TimeToEntityTime(direction.Daytype.AllFootnotes, sjt);
					column.Times.Add(time);
				}
				columns.Add(column);
			}

			return columns
				.Where(c => c.Times.Any(t => !string.IsNullOrEmpty(t.Text)))
				.ToArray();
		}

		private static Footnote[] DirectionToFootnotes(InitDirection direction)
		{
			var fns = direction?.Daytype?.AllFootnotes;

			if (fns?.Footnotes?.Length > 0)
			{
				return fns
					.Footnotes
					.Select(fn => fn.Symbol)
					.Select(s => SymbolToEntityFootnote(fns, s))
					.Where(s => s != null)
					.ToArray() as Footnote[];
			}
			return Enumerable
				.Empty<Footnote>()
				.ToArray();
		}

		private static Time TimeToEntityTime(InitAllFootnotes footnotes, InitTime time)
		{
			if (string.IsNullOrEmpty(time?.Text))
			{
				return new Time();
			}

			var footNotes = (time.Footnotes ?? Enumerable.Empty<string>())
				.Select(symbol => SymbolToEntityFootnote(footnotes, symbol))
				.Where(fn => fn != null)
				.ToArray() as Footnote[];

			return new Time
			{
				Text = time.Text.Trim('A', 'a', 'P', 'p', '-'),
				Footnotes = footNotes,
				IsAm = time.Text.EndsWith("A", StringComparison.OrdinalIgnoreCase),
				IsPm = time.Text.EndsWith("P", StringComparison.OrdinalIgnoreCase),
				IsHopper = time.IsHopper,
				IsDashed = time
					.Text
					.ToCharArray()
					.All(c => c == '-')
			};
		}

		private static Footnote? SymbolToEntityFootnote(InitAllFootnotes footnotes, string symbol)
		{
			var fn = footnotes.Footnotes.FirstOrDefault(s => s.Symbol == symbol);
			if (fn != null)
			{
				var converted = ConvertSymbol(symbol);
				var anchor = GetAnchorFromSymbol(converted);
				return new Footnote
				{
					Anchor = GetAnchorFromSymbol(anchor),
					Symbol = ConvertSymbol(converted),
					Text = fn.Content
				};
			}
			return null;
		}

		private async Task<ParentStop?> GetStopByIdOrDefaultAsync(string stopId, CancellationToken cancellationToken)
		{
			try
			{
				var result = await stopwatchContext
					.ParentStops
					.SingleOrDefaultAsync(parentStop => parentStop.Id.CompareTo(stopId) == 0, cancellationToken)
					.ConfigureAwait(false);
				return result;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to fetch stop {stopId}", stopId);
			}
			return null;
		}

		private static string GetAnchorFromSymbol(string symbol) => symbol switch
		{
			"🞛" => "eor",
			_ => symbol,
		};

		private static string ConvertSymbol(string symbol) => symbol switch
		{
			"#" => "🞛",
			_ => symbol,
		};

	}
}
