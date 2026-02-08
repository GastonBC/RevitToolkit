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
            var selection = this.UiDocument.Selection.GetElementIds()
                                   .Select(id => this.Document.GetElement(id)).ToList();

            if (!selection.Any())
            {
                TaskDialog.Show("Error", "Please select elements first.");
                return;
            }

            var window = new ReValueWindow(this.Document, selection);
            window.ShowDialog();

            return;
        }
    }
}