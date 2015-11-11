using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.CustomLayoutRenderers
{
	[LayoutRenderer("userId")]
	public class HelloWorldLayoutRenderer : LayoutRenderer
	{
		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			builder.Append(1);
		}
	}
}
