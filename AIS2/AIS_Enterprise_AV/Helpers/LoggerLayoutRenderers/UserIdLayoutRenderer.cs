using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_AV.Auth;
using NLog;
using NLog.LayoutRenderers;

namespace AIS_Enterprise_AV.Helpers.LoggerLayoutRenderers
{
	[LayoutRenderer("userId")]
	public class UserIdLayoutRenderer : LayoutRenderer
	{
		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			builder.Append(Privileges.UserId);
		}
	}
}
