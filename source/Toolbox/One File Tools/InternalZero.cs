using Autodesk.Revit.DB;
using System.Windows;

namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void InternalZero_Click(object sender, RoutedEventArgs e)
        {
            Close();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Internal Zero");

                XYZ ZeroZero = new XYZ(0, 0, 0);
                XYZ OneOne = new XYZ(1, 1, 0);

                Line L0 = Line.CreateBound(ZeroZero, OneOne);

                try
                {
                    DetailCurve ZeroCurve = doc.FamilyCreate.NewDetailCurve(doc.ActiveView, L0);
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    DetailCurve ZeroCurve = doc.Create.NewDetailCurve(doc.ActiveView, L0);
                }
                t.Commit(); // CRASHES REVIT. Probably because its not IExternalApplication
            }
        }
    }
}
