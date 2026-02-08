using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace ReValue.Commands
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
            var selection = UiDocument.Selection.GetElementIds()
                                   .Select(id => Document.GetElement(id)).ToList();

            if (!selection.Any())
            {
                TaskDialog.Show("Error", "Please select elements first.");
                return;
            }

            var window = new ReValueWindow(Document, selection);
            window.ShowDialog();

            return;
        }
    }
}