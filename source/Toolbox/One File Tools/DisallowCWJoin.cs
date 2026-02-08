using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using System.Windows;


namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void DisallowJoins_Click(object sender, RoutedEventArgs e)
        {
            Close();

            IEnumerable<Wall> CWCol = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .Cast<Wall>().Where(w => w.Name == "CW - A 1 Base");

            using (Transaction t = new Transaction(doc, "Disallow CW Joins"))
            {
                t.Start();

                foreach (Wall wall in CWCol)
                {
                    WallUtils.DisallowWallJoinAtEnd(wall, 0);
                    WallUtils.DisallowWallJoinAtEnd(wall, 1);
                }

                t.Commit();
            }
        }
    }
}

