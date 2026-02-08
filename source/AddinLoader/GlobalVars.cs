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
        // Gets the full path to the current DLL file
        internal static string assemblyLocation = Assembly.GetExecutingAssembly().Location;

        internal static string currentDllDir = Path.GetDirectoryName(assemblyLocation); // This assembly location

        internal static string ADDINS_DLL_PATH = Directory.GetParent(Path.GetDirectoryName(assemblyLocation)).FullName;


        internal static string INVOKE01_PATH = Path.Combine(ADDINS_DLL_PATH, "TypeRenamer", "TypeRenamer.dll");
        internal static string INVOKE02_PATH = Path.Combine(ADDINS_DLL_PATH, "FixElementConstraints", "FixElementConstraints.dll");
        internal static string INVOKE03_PATH = Path.Combine(ADDINS_DLL_PATH, "ReValue", "ReValue.dll");
    }
}
