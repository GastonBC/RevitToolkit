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


        //internal static string RENAMER_PATH = Path.Combine(ADDINS_DLL_PATH, "TypeRenamer", "TypeRenamer.dll");
        //internal static string FIXCONST_PATH = Path.Combine(ADDINS_DLL_PATH, "FixElementConstraints", "FixElementConstraints.dll");
        //internal static string REVALUE_PATH = Path.Combine(ADDINS_DLL_PATH, "ReValue", "ReValue.dll");
        //internal static string CADDET_PATH = Path.Combine(ADDINS_DLL_PATH, "CADDetective", "CADDetective.dll");
        //internal static string CROPREG_PATH = Path.Combine(ADDINS_DLL_PATH, "CropReg", "CropReg.dll");

        //internal static string EVR_PATH = Path.Combine(ADDINS_DLL_PATH, "ElementViewRange", "ElementViewRange.dll");
        //internal static string FINDSCHEDS_PATH = Path.Combine(ADDINS_DLL_PATH, "FindSchedsLegends", "FindSchedsLegends.dll");
        //internal static string ORIENTBOX_PATH = Path.Combine(ADDINS_DLL_PATH, "OrientBoxToFace", "OrientBoxToFace.dll");
        //internal static string PCNORM_PATH = Path.Combine(ADDINS_DLL_PATH, "PointCloudNormals", "PointCloudNormals.dll");
        //internal static string SETINDEX_PATH = Path.Combine(ADDINS_DLL_PATH, "SetByIndex", "SetByIndex.dll");

        //internal static string PCTOGGLE_PATH = Path.Combine(ADDINS_DLL_PATH, "TogglePointCloud", "TogglePointCloud.dll");
        //internal static string TOOLS_PATH = Path.Combine(ADDINS_DLL_PATH, "Toolbox", "Toolbox.dll");
        //internal static string EXTENTS_PATH = Path.Combine(ADDINS_DLL_PATH, "MatchGridExtents", "MatchGridExtents.dll");
        //internal static string DIMGRIDS_PATH = Path.Combine(ADDINS_DLL_PATH, "AutoDimGrids", "AutoDimGrids.dll");
        //internal static string SMARTGRIDS_PATH = Path.Combine(ADDINS_DLL_PATH, "SmartGridBubbles", "SmartGridBubbles.dll");
    }
}
