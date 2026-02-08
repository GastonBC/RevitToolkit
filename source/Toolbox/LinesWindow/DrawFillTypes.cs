using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Toolbox
{
    public partial class DrawLinesWindow : Window
    {
        private void FillType_Click()
        {
            Close();


            View activeView = doc.ActiveView;

            IOrderedEnumerable<Element> AllFillTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(FilledRegionType))
                .WhereElementIsElementType()
                .OrderByDescending(p => p.Name); // ordered patterns by name

            double xMainPos = 15d;
            ElementId TagStyle = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

            using (Transaction t = new Transaction(doc))
            {


                XYZ V1 = new XYZ(xMainPos + 0, 0, 0);      // Diagonal line
                XYZ V3 = new XYZ(xMainPos + (2 * 1.618), 2d, 0);

                XYZ V2 = new XYZ(V3.X, V1.Y, 0);
                XYZ V4 = new XYZ(V1.X, V3.Y, 0);

                t.Start("Draw fill region types");
                foreach (Element type in AllFillTypes)
                {
                    List<CurveLoop> profileloops = new List<CurveLoop>();


                    Line L1 = Line.CreateBound(V1, V2);
                    Line L2 = Line.CreateBound(V2, V3);
                    Line L3 = Line.CreateBound(V3, V4);
                    Line L4 = Line.CreateBound(V4, V1);

                    List<Curve> LineList = new List<Curve>() { L1, L2, L3, L4 };

                    CurveLoop CrvLoop = CurveLoop.Create(LineList);

                    profileloops.Add(CrvLoop);

                    FilledRegion NewRegion = FilledRegion.Create(doc, type.Id, activeView.Id, profileloops);


                    TextNote.Create(doc,
                                    activeView.Id,
                                    new XYZ(V2.X + 0.5, V2.Y + 1, V2.Z),
                                    type.Name,
                                    TagStyle);

                    XYZ MoveUp = new XYZ(0, 2.8, 0);

                    V1 = V1.Add(MoveUp);
                    V2 = V2.Add(MoveUp);
                    V3 = V3.Add(MoveUp);
                    V4 = V4.Add(MoveUp);

                }
                t.Commit();
            }
        }
    }
}
