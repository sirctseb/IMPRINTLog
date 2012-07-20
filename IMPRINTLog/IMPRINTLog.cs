using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMPRINTLog
{
	public class SmartPlugin
	{
		public static MAAD.Simulator.Utilities.ISimulationApplication app
		{
			get { return MAAD.Simulator.Generator.StaticGenerator.SimulationApplication; }
		}
	}
	public class IMPRINTLog : SmartPlugin
	{
		public IMPRINTLog()
		{
			// initialize default logger
		}
	}
}
