using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Toolbox
{
    public partial class DrawLinesWindow : Window
    {
        private void FillPattern_Click()
        {
            Close();

            View activeView = doc.ActiveView;

            IOrderedEnumerable<FillPatternElement> AllFillPatterns = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement))
                .ToElements()
                .Cast<FillPatternElement>()
                .Where(p => p.GetFillPattern().Target == FillPatternTarget.Drafting) // Override only works with drafting patterns, even in revit...
                .OrderByDescending(p => p.Name); // ordered patterns by name

            ElementId FillRegionDefault = doc.GetDefaultElementTypeId(ElementTypeGroup.FilledRegionType);
            ElementId TagStyle = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

            double xMainPos = 5d;

            using (Transaction t = new Transaction(doc))
            {


                XYZ V1 = new XYZ(xMainPos + 0, 0, 0);      // Diagonal line
                XYZ V3 = new XYZ(xMainPos + 2 * 1.618, 2d, 0);

                XYZ V2 = new XYZ(V3.X, V1.Y, 0);
                XYZ V4 = new XYZ(V1.X, V3.Y, 0);

                t.Start("Draw fill region patterns");
                foreach (FillPatternElement pat in AllFillPatterns)
                {
                    List<CurveLoop> profileloops = new List<CurveLoop>();

                    Line L1 = Line.CreateBound(V1, V2);
                    Line L2 = Line.CreateBound(V2, V3);
                    Line L3 = Line.CreateBound(V3, V4);
                    Line L4 = Line.CreateBound(V4, V1);

                    List<Curve> LineList = new List<Curve>() { L1, L2, L3, L4 };

                    CurveLoop CrvLoop = CurveLoop.Create(LineList);

                    profileloops.Add(CrvLoop);

                    FilledRegion NewRegion = FilledRegion.Create(doc, FillRegionDefault, activeView.Id, profileloops);

                    OverrideGraphicSettings overrideGraphicSettings = new OverrideGraphicSettings();
                    overrideGraphicSettings.SetSurfaceForegroundPatternId(pat.Id);
                    overrideGraphicSettings.SetSurfaceForegroundPatternColor(new Color(0, 0, 0));

                    overrideGraphicSettings.SetSurfaceBackgroundPatternVisible(false);

                    activeView.SetElementOverrides(NewRegion.Id, overrideGraphicSettings);

                    TextNote.Create(doc,
                                    activeView.Id,
                                    new XYZ(V2.X + 0.5, V2.Y + 1, V2.Z),
                                    pat.Name,
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