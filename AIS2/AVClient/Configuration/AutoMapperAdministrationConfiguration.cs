using AutoMapper;
using AVClient.AVServiceReference;
using AVClient.Models.Administration;

namespace AVClient.Configuration
{
	public static class AutoMapperAdministrationConfiguration
	{
		public static void Configurate()
		{
			Mapper.CreateMap<DTOUser, User>();
		}
	}
}