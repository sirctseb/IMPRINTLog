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
		void AcceptTrace(Object obj);

		// Do any work required when the simulation ends
		void Finalize();
	}

	// Class for logging to IMPRINT output window
	public class IMPRINTOutputWindow : SmartPlugin, ILogConsole
	{
		public void AcceptTrace(Object obj)
		{
			app.AcceptTrace(obj);
		}

		public void Finalize() { }
	}

	// Class for logging to a file
	public class FileLog : ILogConsole
	{
		private String FilePath;
		private System.IO.StreamWriter FileWriter;

		public FileLog(String FilePath)
		{
			this.FilePath = FilePath;

			// Construct stream writer
			FileWriter = new System.IO.StreamWriter(FilePath);
		}

		public void AcceptTrace(Object obj)
		{
			FileWriter.WriteLine(obj.ToString());
		}

		public void Finalize()
		{
			// close file writer
			FileWriter.Close();
		}
	}

	public class IMPRINTLog : SmartPlugin
	{
		public ILogConsole Console
		{
			get; set;
		}

		// Default constructor
		public IMPRINTLog()
		{
			// initialize default logger
			Console = new IMPRINTOutputWindow();
		}

		// Construct with a name and a console
		public IMPRINTLog(String Name, ILogConsole Console)
		{
			this.Name = Name;
			this.Console = Console;
		}

		#region Static members for accessing different logs

		// map from log names to the logs
		private static Dictionary<String, IMPRINTLog> logs = new Dictionary<String, IMPRINTLog>();

		public static IMPRINTLog GetLog(String LogName)
		{
			if (logs.ContainsKey(LogName)) return logs[LogName];

			// TODO what to do if the log doesn't exist?
			// return null?
			// throw exception?
			// return default logger?
			// create new logger with default values and return it?
			return null;
		}

		// Create a log with a name and a console
		public static IMPRINTLog CreateLog(String Name, ILogConsole Console)
		{
			return new IMPRINTLog(Name, Console);
		}

		// Create a log with a name and a file path
		public static IMPRINTLog CreateLog(String Name, String FilePath)
		{
			return new IMPRINTLog(Name, new FileLog(FilePath));
		}

		#endregion

		#region Instance members

		// exception for when we try to set the name to one that already exists
		public class DuplicateNameException : Exception {
			public DuplicateNameException(String name) : base("Log named " + name + " already exists") {}
		}
		private String name = "default";
		// The name of the log
		public String Name
		{
			get { return name; }
			set
			{
				// Don't allow changing to a name that already exists
				if (logs.ContainsKey(value))
				{
					// TODO throw exception?
					throw new DuplicateNameException(value);
				}

				// remove old name from map
				logs.Remove(name);
				// set backing field
				name = value;
				// add new name to map
				logs[name] = this;
			}
		}

		// The current logging level
		public int LogLevel { get; set; }

		// The logging groups that are enabled
		private HashSet<String> groups;

		#region Group enabling/disabling

		// Enable a logging group
		// return true if the group was previously disabled and is now enabled
		public bool EnableGroup(String group) {
			return groups.Add(group);
		}

		// Disable a logging group
		// return false if the group was enabled and is now disabled
		public bool DisableGroup(String group)
		{
			return groups.Remove(group);
		}

		// Disable all logging groups
		public void DisableAllGroups()
		{
			groups.Clear();
		}

		#endregion

		// Log methods return true iff the content is actually sent to the console

		#region Log methods

		// Unqualified log. always log
		public bool Log(Object message)
		{
			console.AcceptTrace(message);
			return true;
		}

		// Log if the supplied level is less than the current log level
		public bool Log(Object message, int Level)
		{
			if (Level <= LogLevel)
			{
				return Log(message);
			}

			return false;
		}

		// Log if the supplied group is enabled
		public bool Log(Object message, String group)
		{
			if (groups.Contains(group))
			{
				return Log(message);
			}

			return false;
		}

		#endregion

		#endregion
	}
}
