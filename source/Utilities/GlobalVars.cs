using System.IO;
using System.Reflection;

namespace Utilities
{
    public static class GlobalVars
    {
        public static string TAB_NAME = "Gas Tools";

        // Gets the full path to the current DLL file
        public static string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        public static string currentDllDir = Path.GetDirectoryName(assemblyLocation); // This assembly location
        public static string ADDINS_DLL_PATH = Directory.GetParent(Path.GetDirectoryName(assemblyLocation)).FullName;
    }
}
