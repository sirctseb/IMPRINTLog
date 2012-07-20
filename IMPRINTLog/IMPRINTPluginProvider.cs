using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MAAD.Plugins.ImprintPluginLoader;

[assembly: ImprintPluginAttribute()]


namespace IMPRINTLogNamespace
{
	public class IMPRINTLogPlugin : IExternalVariableProvider, IImprintPlugin
	{
		#region IExternalVariableProvider Implementation

		public string[] GetAssemblyReferences()
		{
			return new string[]
			{
				"IMPRINTLog.dll"
			};
		}

		public IEnumerable<VariableName> GetExternalVariables()
		{
			IMPRINTLog.RegisterEvents();

			return new VariableName[]
			{
				new VariableName("IMPRINTLog", typeof(IMPRINTLog))
			};
		}

		public string[] GetNamespaceAliases()
		{
			return new string[]
			{
				"IMPRINTLogNamespace"
			};
		}

		#endregion

		#region IImprintPlugin Implementation
		
		public string Author
		{
			get { return "Christopher Best"; }
		}

		public DateTime Date
		{
			get { return DateTime.Today; }
		}

		public string Description
		{
			get { return "A logging class for IMPRINT"; }
		}

		public string Name
		{
			get { return "IMPRINTLog"; }
		}

		#endregion
	}
}
