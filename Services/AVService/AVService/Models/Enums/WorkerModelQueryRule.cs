using System;

namespace AVService.Models.Enums
{
	[Flags]
	public enum WorkerModelQueryRule
	{
		None = 0,
		CurrentPosts = 1,
		DirectoryPosts = 2,
		InfoDates = 4,
		InfoMonths = 8
	}
}