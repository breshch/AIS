using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace AIS_Enterprise_AV.Helpers.LoggerLayoutRenderers
{
	public static class LoggerConfiguration
	{
		public static void Configurate()
		{
			ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("userId", typeof(UserIdLayoutRenderer));
			ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("application", typeof(ApplicationLayoutRenderer));
		}
	}
}
