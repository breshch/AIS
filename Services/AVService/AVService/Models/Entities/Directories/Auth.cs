namespace AVService.Models.Entities.Directories
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
