namespace AVRepository.Repositories
{
	public class BaseRepository
	{
		public DataContext GetContext()
		{
			return new DataContext();
		}
	}
}