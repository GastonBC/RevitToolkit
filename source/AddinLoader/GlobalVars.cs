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

        internal static string INVOKE04_PATH = Path.Combine(ADDINS_DLL_PATH, "CADDetective", "CADDetective.dll");
        internal static string INVOKE05_PATH = Path.Combine(ADDINS_DLL_PATH, "CropReg", "CropReg.dll");
        internal static string INVOKE06_PATH = Path.Combine(ADDINS_DLL_PATH, "ElementViewRange", "ElementViewRange.dll");

        internal static string INVOKE07_PATH = Path.Combine(ADDINS_DLL_PATH, "FindSchedsLegends", "FindSchedsLegends.dll");
        internal static string INVOKE08_PATH = Path.Combine(ADDINS_DLL_PATH, "OrientBoxToFace", "OrientBoxToFace.dll");
        internal static string INVOKE09_PATH = Path.Combine(ADDINS_DLL_PATH, "PointCloudNormals", "PointCloudNormals.dll");

        internal static string INVOKE10_PATH = Path.Combine(ADDINS_DLL_PATH, "ReValue", "ReValue.dll");
        internal static string INVOKE11_PATH = Path.Combine(ADDINS_DLL_PATH, "SetByIndex", "SetByIndex.dll");
        internal static string INVOKE12_PATH = Path.Combine(ADDINS_DLL_PATH, "TogglePointCloud", "TogglePointCloud.dll");

        internal static string INVOKE13_PATH = Path.Combine(ADDINS_DLL_PATH, "Toolbox", "Toolbox.dll");
        internal static string INVOKE14_PATH = Path.Combine(ADDINS_DLL_PATH, "OrientBoxToFace", "OrientBoxToFace.dll");
        internal static string INVOKE15_PATH = Path.Combine(ADDINS_DLL_PATH, "PointCloudNormals", "PointCloudNormals.dll");
    }
}
