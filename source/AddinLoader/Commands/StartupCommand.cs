using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace AddinLoader.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Invoke01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Utils.InvokeCmd(commandData, ref message, elements,
                                    GlobalVars.INVOKE01_PATH, "INVOKE 01");
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Invoke02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Utils.InvokeCmd(commandData, ref message, elements,
                                    GlobalVars.INVOKE02_PATH, "INVOKE 02");
            return Result.Succeeded;
        }
    }
}