using System;

namespace AVService.Models.Entities.Directories
{
//	[Table("DirectoryPAM16Percentages")]
	public class DirectoryPAM16Percentage
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public double  Percentage { get; set; }
	}
}
