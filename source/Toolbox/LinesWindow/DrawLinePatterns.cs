using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Toolbox
{
    public partial class DrawLinesWindow : Window
    {
        private void LinePattern_Click()
        {
            Close();

            double xMainPos = -25d;

            double[] xyzStart = { xMainPos + 0, 0, 0 };
            double[] xyzEnd = { xMainPos + 10, 0, 0 };
            View activeView = doc.ActiveView;

            IOrderedEnumerable<Element> all_pat = new FilteredElementCollector(doc).
                OfClass(typeof(LinePatternElement)).ToElements().OrderByDescending(p => p.Name); // ordered patterns by name

            ElementId TagStyle = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Draw all patterns");
                foreach (Element EachPat in all_pat)
                {
                    LinePatternElement PatternElem = EachPat as LinePatternElement;
                    LinePattern Pattern = PatternElem.GetLinePattern();

                    OverrideGraphicSettings overrideGraphicSettings = new OverrideGraphicSettings();
                    overrideGraphicSettings.SetProjectionLinePatternId(PatternElem.Id);

                    xyzStart[1] = xyzStart[1] + 0.5;
                    xyzEnd[1] = xyzEnd[1] + 0.5;



                    Line L0 = Line.CreateBound(new XYZ(xyzStart[0], xyzStart[1], xyzStart[2]),
                                               new XYZ(xyzEnd[0], xyzEnd[1], xyzEnd[2]));

                    DetailCurve detailLine = doc.Create.NewDetailCurve(activeView, L0);

                    TextNote.Create(doc,
                                    activeView.Id,
                                    new XYZ(xyzStart[0], xyzStart[1] + 0.1, xyzStart[2]),
                                    Pattern.Name,
                                    TagStyle);

                    activeView.SetElementOverrides(detailLine.Id, overrideGraphicSettings);
                }
                t.Commit();
            }
        }
    }
}
