using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Infos;

namespace AVService.Models.Repositories
{
	public class WorkerModel
	{
		public DirectoryWorker Worker { get; set; }
		public CurrentPost[] CurrentPosts { get; set; }
		public DirectoryPost[] DirectoryPosts { get; set; }
		public InfoDate[] InfoDates { get; set; }
		public InfoMonth[] InfoMonthes { get; set; }
	}
}
