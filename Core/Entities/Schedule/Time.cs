namespace WebScheduleGenerator.Core.Entities.Schedule;

public class Time
{
	private string? _text;

	public string? Text
	{
		get => _text;
		set => _text = string.IsNullOrEmpty(value) ? null : value;
	}

	public Footnote[]? Footnotes { get; set; }
	public bool IsHopper { get; set; }
	public bool IsDashed { get; set; }
	public bool IsAm { get; set; }
	public bool IsPm { get; set; }
}
