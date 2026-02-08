using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace TogglePointCloud.Commands
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
            View view = doc.ActiveView;

            using (Transaction t = new Transaction(doc, "Toggle Point Clouds"))
            {
                t.Start();

                FilteredElementCollector collector = new FilteredElementCollector(doc)
                    .OfClass(typeof(PointCloudInstance));

                if (collector.First().IsHidden(view))
                {
                    ICollection<ElementId> pointCloudIds = collector.ToElementIds();
                    view.UnhideElements(pointCloudIds);
                }

                else
                {
                    ICollection<ElementId> pointCloudIds = collector.ToElementIds();
                    view.HideElements(pointCloudIds);
                }
                t.Commit();
            }
        }
    }
}