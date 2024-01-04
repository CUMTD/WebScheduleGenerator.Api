using System.ComponentModel.DataAnnotations;

namespace WebScheduleGenerator.Api.Config
{
	internal class Security
	{
		[Required]
		public string[] ApiKeys { get; set; } = default!;
	}
}
