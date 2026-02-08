using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;

namespace Toolbox
{
    /// <summary>
    /// Interaction logic for DrawLinesWindow.xaml
    /// </summary>
    public partial class DrawLinesWindow : Window
    {
        UIDocument uidoc = null;
        Document doc = null;

        public DrawLinesWindow(UIDocument uid)
        {
            uidoc = uid;
            doc = uidoc.Document;

            InitializeComponent();
        }


        // "PreClick"s are just so I can merge all into DoAll button

        private void DoAll_Click(object sender, RoutedEventArgs e)
        {
            LinePattern_Click();
            LineStyle_Click();
            FillPattern_Click();
            FillType_Click();
        }

        private void LinePattern_PreClick(object sender, RoutedEventArgs e)
        {
            LinePattern_Click();
        }

        private void LineStyle_PreClick(object sender, RoutedEventArgs e)
        {
            LineStyle_Click();
        }

        private void FillPattern_PreClick(object sender, RoutedEventArgs e)
        {
            FillPattern_Click();
        }

        private void FillType_PreClick(object sender, RoutedEventArgs e)
        {
            FillType_Click();
        }
    }
}
