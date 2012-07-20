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

	// Interface for classes that we can log to
	public interface ILogConsole
	{
		// Accept an object to add to the log
		void AcceptTrace(object obj);
	}

	// Class for logging to IMPRINT output window
	public class IMPRINTOutputWindow : SmartPlugin, ILogConsole
	{
		public void AcceptTrace(object obj)
		{
			app.AcceptTrace(obj);
		}
	}

	public class IMPRINTLog : SmartPlugin
	{
		public ILogConsole console
		{
			get; set;
		}

		// default constructor
		public IMPRINTLog()
		{
			// initialize default logger
			console = new IMPRINTOutputWindow();
		}
	}
}
