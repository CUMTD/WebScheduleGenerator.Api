using System.ComponentModel.DataAnnotations;

namespace WebScheduleGenerator.Api.Config
{
	internal class Cors
	{
		[Required]
		public string PolicyName { get; set; } = default!;
	}
}
