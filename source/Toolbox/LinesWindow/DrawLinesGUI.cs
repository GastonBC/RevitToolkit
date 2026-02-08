using Autodesk.Revit.DB;
using System.Windows;
using Utilities;

namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void DrawLines_Click(object sender, RoutedEventArgs e)
        {
            Close();
            View activeView = doc.ActiveView;

            if (activeView.ViewType == ViewType.DraftingView)
            {
                DrawLinesWindow MainWindow = new DrawLinesWindow(uidoc);
                MainWindow.ShowDialog(); // TODO Rename WindowN class to something useful?
            }
            else
            {
                Utils.SimpleDialog("Go to a drafting view", "");
            }
        }
    }
}
