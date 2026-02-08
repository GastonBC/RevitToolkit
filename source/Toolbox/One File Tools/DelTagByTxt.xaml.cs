using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Toolbox.One_file_tools
{
    public partial class DelTagByTxt : Window
    {
        UIDocument _uidoc;
        Document _doc;

        public DelTagByTxt(UIDocument uidoc, Document doc)
        {
            _uidoc = uidoc;
            _doc = doc;
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            IList<Element> TagCol = new FilteredElementCollector(_doc)
                .OfClass(typeof(IndependentTag))
                .ToElements();

            using (Transaction t = new Transaction(_doc, $"Delete {SetNameBox.Text} tags"))
            {
                t.Start();
                foreach (IndependentTag tag in TagCol)
                {
                    if (tag.TagText == SetNameBox.Text)
                    {
                        _doc.Delete(tag.Id);
                    }
                }
                t.Commit();
            }
        }
    }
}
