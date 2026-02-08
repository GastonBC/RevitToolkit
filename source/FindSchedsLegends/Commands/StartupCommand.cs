using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using System.Xml.Linq;
using Utilities;

namespace FindSchedsLegends.Commands
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

            // ViewSheets
            FilteredElementCollector SheetCollector = new FilteredElementCollector(doc);
            SheetCollector.OfCategory(BuiltInCategory.OST_Sheets)
                          .WhereElementIsNotElementType()
                          .Cast<ViewSheet>()
                          .Where(sheet => !sheet.IsPlaceholder);

            // Get all placed schedules
            FilteredElementCollector SchedCollector = new FilteredElementCollector(doc);
            SchedCollector.OfCategory(BuiltInCategory.OST_ScheduleGraphics)
                          .WhereElementIsNotElementType()
                          .OfType<ScheduleSheetInstance>();


            // If user selected something, check if they are viewschedules or view
            ICollection<ElementId> selectedIds = UiDocument.Selection.GetElementIds();
            try
            {
                if (selectedIds.Count == 0)
                {
                    FindSLMainWn FindSLGUI = new FindSLMainWn(UiDocument,
                                                              SheetCollector,
                                                              SchedCollector);
                    FindSLGUI.ShowDialog();
                }

                else
                {
                    // Check if selection is viewscheds or legends (views)

                    List<Element> FilteredList = new List<Element>();

                    foreach (ElementId ElemId in selectedIds)
                    {
                        Element SelectedElem = doc.GetElement(ElemId);
                        if (SelectedElem is View || SelectedElem is ViewSchedule)
                        {
                            FilteredList.Add(SelectedElem);
                        }
                    }

                    if (FilteredList.Count != 0)
                    {
                        FindSLMainWn FindSLGUI = new FindSLMainWn(UiDocument,
                                                                  SheetCollector,
                                                                  SchedCollector,
                                                                  FilteredList);
                        FindSLGUI.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.CatchDialog(ex);
            }
        }
    }
}