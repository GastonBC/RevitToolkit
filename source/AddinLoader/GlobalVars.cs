using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AddinLoader
{
    internal static class GlobalVars
    {
        internal static string TAB_NAME = "Gas Tools";

        // Gets the full path to the current DLL file
        internal static string assemblyLocation = Assembly.GetExecutingAssembly().Location;

        internal static string currentDllDir = Path.GetDirectoryName(assemblyLocation); // This assembly location

        internal static string ADDINS_DLL_PATH = Directory.GetParent(Path.GetDirectoryName(assemblyLocation)).FullName;
    }
}
