using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Toolbox.One_file_tools
{
    public partial class ReplaceDimTxt : Window
    {
        UIDocument _uidoc;
        Document _doc;
        public ReplaceDimTxt(UIDocument uidoc, Document doc)
        {
            _uidoc = uidoc;
            _doc = doc;
            InitializeComponent();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ICollection<ElementId> UserSelection = _uidoc.Selection.GetElementIds();

            using (Transaction t = new Transaction(_doc, "Replace dimension value"))
            {
                t.Start();
                foreach (ElementId elemId in UserSelection)
                {
                    Element elem = _doc.GetElement(elemId);
                    if (elem is Dimension)
                    {
                        Dimension DimElem = elem as Dimension;

                        foreach (DimensionSegment segm in DimElem.Segments)
                        {
                            segm.ValueOverride = SetNameBox.Text;
                        }

                    }
                }
                t.Commit();
            }
        }
    }
}
