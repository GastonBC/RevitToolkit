using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Toolbox
{
    public partial class DrawLinesWindow : Window
    {
        private void LineStyle_Click()
        {
            Close();

            double xMainPos = -10d;

            double[] xyzStart = { xMainPos + 0, 0, 0 };
            double[] xyzEnd = { xMainPos + 10, 0, 0 };
            View activeView = doc.ActiveView;

            ElementId TagStyle = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Get line styles");
                Line L0 = Line.CreateBound(new XYZ(0, 0, 0),
                                           new XYZ(1, 1, 0));

                DetailCurve detailLine = doc.Create.NewDetailCurve(activeView, L0);
                ICollection<ElementId> AllStylesIds = detailLine.GetLineStyleIds();
                t.RollBack();

                List<Element> AllStyles = new List<Element>();

                foreach (ElementId StyleId in AllStylesIds)
                {
                    AllStyles.Add(doc.GetElement(StyleId));
                }

                t.Start("Draw all Styles");
                foreach (Element Style in AllStyles.OrderByDescending(s => s.Name))
                {
                    Element StyleElem = doc.GetElement(Style.Id);
                    xyzStart[1] = xyzStart[1] + 0.5;
                    xyzEnd[1] = xyzEnd[1] + 0.5;

                    Line L0bound = Line.CreateBound(new XYZ(xyzStart[0], xyzStart[1], xyzStart[2]),
                                               new XYZ(xyzEnd[0], xyzEnd[1], xyzEnd[2]));

                    DetailCurve L0detailLine = doc.Create.NewDetailCurve(doc.ActiveView, L0bound);

                    TextNote.Create(doc,
                                    activeView.Id,
                                    new XYZ(xyzStart[0], xyzStart[1] + 0.1, xyzStart[2]),
                                    StyleElem.Name,
                                    TagStyle);
                    L0detailLine.LineStyle = StyleElem;
                }
                t.Commit();
            }
        }
    }
}