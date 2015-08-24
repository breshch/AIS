using AVClient.AVServiceReference;

namespace AVClient
{
	public static class ServerConnector
	{
		private static readonly object lockInstanse = new object();

		private static AVBusinessLayerClient _instance;

		static ServerConnector()
		{
		}

		public static AVBusinessLayerClient GetInstanse 
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				lock (lockInstanse)
				{
					if (_instance != null)
					{
						return _instance;
					}

					_instance = new AVBusinessLayerClient();
				}

				return _instance;
			}
		}
	}
}
