using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.LayoutRenderers;

namespace AIS_Enterprise_AV.Helpers.LoggerLayoutRenderers
{
	[LayoutRenderer("application")]
	public class ApplicationLayoutRenderer : LayoutRenderer
	{
		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			builder.Append("empty");
		}
	}
}
