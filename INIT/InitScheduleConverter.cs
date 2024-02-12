using System.Text.RegularExpressions;
using WebScheduleGenerator.Core;
using WebScheduleGenerator.Core.Entities.Schedule;
using WebScheduleGenerator.Init.Serialization;

namespace WebScheduleGenerator.Init
{
	public partial class InitScheduleConverter() : IScheduleConverter<InitTimetable>
	{
		[GeneratedRegex(@"\(([^)]*)\)")]
		private static partial Regex ParenthesesRegex();

		public Task<ProcessingResult> ConvertScheduleAsync(InitTimetable schedule, CancellationToken cancellationToken)
		{
			var timetables = new List<Timetable>();
			foreach (var direction in schedule.Route.Directions)
			{
				var converted = DirectionToTimetable(schedule, direction, cancellationToken);
				timetables.Add(converted);
			}

			return Task.FromResult(new ProcessingResult(timetables));
		}

		public Task<ProcessingResult> ConvertScheduleAsync(Stream schedule, CancellationToken cancellationToken)
		{
			var timetable = InitXmlParser.Deserialize(schedule);
			return ConvertScheduleAsync(timetable, cancellationToken);
		}

		private Timetable DirectionToTimetable(InitTimetable timetable, InitDirection direction, CancellationToken cancellationToken)
		{
			var stops = DirectionToStops(direction, cancellationToken);

			return new Timetable()
			{
				Route = RouteInfoToRoute(timetable.Route),
				Direction = direction.ShortDirection,
				Stops = stops,
				Rows = DirectionToRows(direction),
				Footnotes = DirectionToFootnotes(direction)
			};
		}

		private Stop[] DirectionToStops(InitDirection direction, CancellationToken cancellationToken)
		{
			var stops = new List<Stop>();
			foreach (var stop in direction.Daytype.Stops)
			{
				var converted = StopToEntityStop(stop, cancellationToken);

				stops.Add(converted);
			}
			return [.. stops];
		}

		private Stop StopToEntityStop(InitStop stop, CancellationToken cancellationToken) => new()
		{
			Id = stop.Id,
			// remove  eveything in parentheses from the name
			Name = ParenthesesRegex().Replace(stop.Name, string.Empty).Trim(),
			Parentheses = GetParentheses(stop.Name),
			SmsCode = stop.SmsCode,
			TimePointLetter = stop.TimePointLetter
		};

		private static Route RouteInfoToRoute(InitRoute route) => new()
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
				var times = new List<Time>();
				foreach (var sjt in sj.Times)
				{
					var time = TimeToEntityTime(direction.Daytype.AllFootnotes, sjt);
					times.Add(time);
				}

				if (times.Any(t => !string.IsNullOrWhiteSpace(t.Text) || t.Footnotes?.Length > 0 || t.IsDashed))
				{
					columns.Add(Row.Create(times));
				}
				else
				{
					columns.Add(Row.CreateEmpty());
				}
			}

			return [.. columns];
		}

		private static Footnote[] DirectionToFootnotes(InitDirection direction)
		{
			var fns = direction?.Daytype?.AllFootnotes;

			return fns
				?.Footnotes
				?.Select(fn => fn.Symbol)
				.Select(s => SymbolToEntityFootnote(fns, s))
				.Where(s => s != null)
				.ToArray() as Footnote[] ?? [];
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
			var fn = footnotes?.Footnotes?.FirstOrDefault(s => s.Symbol == symbol);
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
		private static string GetParentheses(string name) => ParenthesesRegex().Match(name).Groups[1].Value;

	}
}
