namespace WebScheduleGenerator.Core.Entities.Schedule;

public class Timetable
{

	public required string Direction { get; set; }
	public int DirectionNumber => Direction switch
	{
		"EAST" or "NORTH" or "CHAMPAIGN" or "C" or "CLOCKWISE" or "A" => 0,
		"WEST" or "SOUTH" or "URBANA" or "U" or "COUNTERCLOCKWISE" or "B" => 1,
		_ => -1,
	};

	public required Route Route { get; set; }
	public required Stop[] Stops { get; set; }
	public required Row[] Rows { get; set; }
	public required Footnote[] Footnotes { get; set; }
}
