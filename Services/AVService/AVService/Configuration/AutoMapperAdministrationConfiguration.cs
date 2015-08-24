using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVService.Models.DTO.Administration;
using AVService.Models.Entities.Directories;

namespace AVService.Configuration
{
	public  static class AutoMapperAdministrationConfiguration
	{
		public static void Configurate()
		{
			AutoMapper.Mapper.CreateMap<DirectoryUser, DTOUser>();
		}
	}
}
