using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
	public class Auth
	{
		public int Id { get; set; }
		public string Hash { get; set; }
		public string Salt { get; set; }
		
		public int DirectoryUserId { get; set; }
		public virtual DirectoryUser DirectoryUser { get; set; }
	}
}
