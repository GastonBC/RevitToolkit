using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace FixElementConstraints.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            TaskDialog.Show(doc.Title, "FixElems");
            return Result.Succeeded;
        }
    }
}