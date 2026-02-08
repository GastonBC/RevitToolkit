using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace ElementViewRange.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public static PlanViewRange SetVrOffset(PlanViewRange viewRange, PlanViewPlane plane, double ZPoint, Document doc)
        {
            // if the range is invalid, it returns a negative number. Set the plane to the cut plane
            if (viewRange.GetLevelId(plane).Value < 0)
            {
                ElementId cutPlaneId = viewRange.GetLevelId(PlanViewPlane.CutPlane);
                viewRange.SetLevelId(plane, cutPlaneId);
            }

            Level level = doc.GetElement(viewRange.GetLevelId(plane)) as Level;
            double offset = ZPoint - level.Elevation;
            viewRange.SetOffset(plane, offset);

            return viewRange;
        }
        public override void Execute()
        {
            {
                UIDocument uidoc = this.UiDocument;
                Document doc = uidoc.Document;
                Level level;
                List<double> values = new List<double>();
                List<ElementId> idsToDel = new List<ElementId>();


                var selection = uidoc.Selection.GetElementIds();

                if (selection.Count == 0) { return; }

                // Get max and min bounding box points from element selection
                // Values are given in decimal feet, relative to either project origin or shared coordinates
                // All values are returned like this so there is no need for conversion
                foreach (ElementId elemId in selection)
                {
                    Element SelectedElem = doc.GetElement(elemId);
                    BoundingBoxXYZ bbox = SelectedElem.get_BoundingBox(null);
                    values.Add(bbox.Max.Z);
                    values.Add(bbox.Min.Z);

                    // Clean up if its a detail line like most times
                    if (SelectedElem != null && SelectedElem is DetailArc || SelectedElem is DetailLine || SelectedElem is DetailCurve)
                    {
                        idsToDel.Add(elemId);
                    }

                    double maxZ = values.Max();
                    double minZ = values.Min();


                    View view = doc.ActiveView;
                    level = view.GenLevel;

                    // Get view type, views to work out are plan and ceiling
                    // Set all levels to the associated level for easy programming
                    // Relative to each view range plan, unless its unlimited in range
                    if (view.ViewType == ViewType.FloorPlan)
                    {
                        ViewPlan viewPlan = view as ViewPlan;
                        PlanViewRange viewRange = viewPlan.GetViewRange();

                        //Top
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.TopClipPlane, maxZ, doc);

                        //Cut, even though its locked in the UI it needs to be set properly
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.CutPlane, maxZ, doc);

                        //Bottom
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.BottomClipPlane, minZ, doc);

                        //Depth
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.ViewDepthPlane, minZ, doc);

                        using (Transaction t = new Transaction(doc, "Adjust view range"))
                        {
                            t.Start();
                            viewPlan.SetViewRange(viewRange);
                            doc.Delete(idsToDel);
                            t.Commit();
                        }
                    }

                    else if (view.ViewType == ViewType.CeilingPlan)
                    {
                        ViewPlan viewPlan = view as ViewPlan;
                        PlanViewRange viewRange = viewPlan.GetViewRange();

                        //Top
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.TopClipPlane, maxZ, doc);

                        //Cut
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.CutPlane, minZ, doc);

                        //Bottom
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.BottomClipPlane, minZ, doc);

                        //Depth
                        viewRange = SetVrOffset(viewRange, PlanViewPlane.ViewDepthPlane, maxZ, doc);

                        using (Transaction t = new Transaction(doc, "Adjust view range"))
                        {
                            t.Start();
                            viewPlan.SetViewRange(viewRange);
                            t.Commit();
                        }
                    }
                }
            }
        }
    }
}
