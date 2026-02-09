using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.Windows.Controls;

namespace AutoDimGrids.Commands
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

            ReferenceArray GridsRefRow = new ReferenceArray();
            ReferenceArray GridsRefCol = new ReferenceArray();

            XYZ TopPoint = null;
            XYZ LeftPoint = null;

            foreach (ElementId elemId in UiDocument.Selection.GetElementIds())
            {
                Element SelectedElem = Document.GetElement(elemId);
                if (SelectedElem is DatumPlane)
                {
                    DatumPlane xGrid = SelectedElem as DatumPlane;

                    IList<Curve> CurvesInGrid = xGrid.GetCurvesInView(DatumExtentType.ViewSpecific, uidoc.ActiveView);

                    Curve cv = CurvesInGrid.First(); // only one segment grids allowed

                    XYZ cvStart = cv.GetEndPoint(0);
                    XYZ cvEnd = cv.GetEndPoint(1);

                    if (Convert.ToInt32(cvStart.Y) == Convert.ToInt32(cvEnd.Y)) // then its a horizontal line, need to round the double bc the comparison will never work
                    {
                        GridsRefRow.Append(new Reference(xGrid));


                        if (LeftPoint is null && cvStart.X < cvEnd.X) // drawn left to right, keep start    X--------->
                        {
                            LeftPoint = cvStart;
                        }

                        else if (LeftPoint is null && cvStart.X > cvEnd.X) // drawn right to left, keep end    <---------X
                        {
                            LeftPoint = cvEnd;
                        }
                    }

                    else if (Convert.ToInt32(cvStart.X) == Convert.ToInt32(cvEnd.X)) // then its a vertical
                    {
                        GridsRefCol.Append(new Reference(xGrid));

                        if (TopPoint is null && cvStart.Y < cvEnd.Y) // drawn bottom to top, keep end
                        {
                            TopPoint = cvEnd;
                        }

                        else if (TopPoint is null && cvStart.Y > cvEnd.Y) // drawn top to bottom, keep start
                        {
                            TopPoint = cvStart;
                        }
                    }



                }
            }

            using (Transaction t = new Transaction(Document, "Auto dimension grids"))
            {
                t.Start();

                if (GridsRefCol.Size >= 2)
                {
                    XYZ SecondTopPoint = new XYZ(TopPoint.X + 10d, TopPoint.Y, TopPoint.Z);

                    Line lineC = Line.CreateBound(TopPoint, SecondTopPoint);
                    Document.Create.NewDimension(UiDocument.ActiveView, lineC, GridsRefCol);
                }

                if (GridsRefRow.Size >= 2)
                {
                    XYZ SecondLeftPoint = new XYZ(LeftPoint.X, LeftPoint.Y + 10d, LeftPoint.Z);

                    Line lineR = Line.CreateBound(LeftPoint, SecondLeftPoint);
                    Document.Create.NewDimension(UiDocument.ActiveView, lineR, GridsRefRow);
                }

                t.Commit();

            }
        }
    }
}