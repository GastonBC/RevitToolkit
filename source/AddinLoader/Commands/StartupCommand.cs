using AddinLoader;
using AddinLoader.Commands;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using Utilities;

namespace AddinLoader.Commands
{
    public abstract class BaseInvoke : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Get the name of the concrete class (e.g., "TypeRenamer")
                string toolName = GetType().Name;

                // Logic for sibling folders
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string addinsRoot = Directory.GetParent(Path.GetDirectoryName(assemblyLocation)).FullName;

                // Build path: .../TypeRenamer/TypeRenamer.dll
                string targetDllPath = Path.Combine(addinsRoot, toolName, $"{toolName}.dll");

                // Invoke using the class name as the LogName
                return Utils.InvokeCmd(commandData, ref message, elements, targetDllPath, toolName);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    // The class name needs to match exactly the dll being invoked. Folder structure is resolved in the base invoke
    [Transaction(TransactionMode.Manual)] public class TypeRenamer : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class FixElementConstraints : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class ReValue : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class CADDetective : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class CropReg : BaseInvoke { }

    [Transaction(TransactionMode.Manual)] public class ElementViewRange : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class FindSchedsLegends : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class OrientBoxToFace : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class PointCloudNormals : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class SetByIndex : BaseInvoke { }

    [Transaction(TransactionMode.Manual)] public class TogglePointCloud : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class Toolbox : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class MatchGridExtents : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class SmartGridBubbles : BaseInvoke { }
    [Transaction(TransactionMode.Manual)] public class AutoDimGrids : BaseInvoke { }
}


