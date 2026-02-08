using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace Toolbox.Commands
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
            UIApplication uiapp = UiApplication;
            _ = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = UiDocument;



            try
            {
                MainWindow MainWindow = new MainWindow(uidoc, this.ExternalCommandData, this.ElementSet);
                MainWindow.ShowDialog();
            }

            catch (Exception ex)
            {
                Utils.CatchDialog(ex);
            }
        }
    }
}