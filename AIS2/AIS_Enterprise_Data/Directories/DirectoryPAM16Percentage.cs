using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
//	[Table("DirectoryPAM16Percentages")]
	public class DirectoryPAM16Percentage
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public double  Percentage { get; set; }
	}
}
