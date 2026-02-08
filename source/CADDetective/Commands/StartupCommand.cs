using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace CADDetective.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            Document doc = UiDocument.Document;
            UIDocument uidoc = UiDocument;

            try
            {
                FindCADsWindow FindCADsGUI = new FindCADsWindow(uidoc);
                FindCADsGUI.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.CatchDialog(ex);
            }
        }
    }
}