using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace SetByIndex.Commands
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
            UIDocument uidoc = this.UiDocument;
            Document doc = uidoc.Document;

            // Get user selection to see if it's a single sheet schedule
            ICollection<ElementId> elementIds = uidoc.Selection.GetElementIds();
            ElementId elementId = elementIds.FirstOrDefault();

            if (elementIds.Count() == 1)
            {
                Element element = doc.GetElement(elementId);

                ViewSchedule viewSched = null;

                // If element is a placed schedule
                if (element is ScheduleSheetInstance)
                {
                    ScheduleSheetInstance SchedInst = element as ScheduleSheetInstance;

                    viewSched = doc.GetElement(
                        SchedInst.ScheduleId) as ViewSchedule;
                }

                // If element is an unplaced schedule (from the view tree)
                else if (element is ViewSchedule)
                {
                    viewSched = element as ViewSchedule;
                }


                if (viewSched != null)
                {
                    MainWindow MainWn = new MainWindow(uidoc, viewSched);
                    MainWn.ShowDialog();

                }

                // Selection is not valid
                else
                {
                    Utils.SimpleDialog("Select a schedule with sheets in it", "");
                }
            }

            // Nothing selected
            else
            {
                Utils.SimpleDialog("Select a schedule with sheets in it", "");
            }
        }
    }
}