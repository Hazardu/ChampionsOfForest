using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChampionsOfForest.Network
{
	public class CommandInitializer
	{
		public static void Init()
		{
			CommandReader.curr_cmd_index = 100;
			CommandReader.commandsObjects_dict.Clear();
			var types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "ChampionsOfForest.Network.Commands");
			foreach (var t in types)
			{
				var initMethod = t.GetMethod("Initialize");
				if (initMethod != null)
					initMethod.Invoke(null, new object[0]);
			}
		}
		private static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
		{
			return
			  assembly.GetTypes()
					  .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass);
		}
	}
}
