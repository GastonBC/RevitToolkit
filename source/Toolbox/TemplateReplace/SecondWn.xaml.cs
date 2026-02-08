using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Toolbox.TemplateReplace
{
    /// <summary>
    /// Interaction logic for SecondWn.xaml
    /// </summary>
    public partial class SecondWn : Window
    {
        UIDocument uidoc = null;
        Document doc = null;

        List<View> TemplatesToReplace = new List<View>();
        List<View> TemplatesCol = new List<View>();

        public SecondWn(UIDocument uid,
                            List<View> _TempsToReplace,
                            List<View> _TempsCollector)
        {
            InitializeComponent();
            uidoc = uid;
            doc = uidoc.Document;


            TemplatesToReplace = _TempsToReplace;

            // Hide selected templates in previous window
            foreach (View Template in _TempsCollector)
            {
                if (_TempsToReplace.Any(t => t.Id == Template.Id))
                {
                    continue;
                }
                else
                {
                    TemplatesCol.Add(Template);
                }
            }

            TemplatesCol.OrderBy(t => t.Name);
            Templates_lv.ItemsSource = TemplatesCol;

        }

        private void b_Select_Click(object sender, RoutedEventArgs e)
        {
            View TemplateChosen = Templates_lv.SelectedItem as View;

            using (Transaction t = new Transaction(doc, "Replace Templates"))
            {
                t.Start();
                List<View> ViewsWithTemplates = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Views)
                    .Cast<View>()
                    .Where(v => v.IsTemplate == false)
                    .Where(v => TemplatesToReplace.Any(vt => vt.Id == v.ViewTemplateId))
                    .ToList();

                foreach (View ViewToReplaceTemp in ViewsWithTemplates)
                {
                    ViewToReplaceTemp.ViewTemplateId = TemplateChosen.Id;
                }
                t.Commit();
            }

            Close();
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            Templates_lv.SelectedItems.Clear();
        }

        private void search_txt_changed(object sender, TextChangedEventArgs e)
        {
            Templates_lv.ItemsSource = TemplatesCol.Where(i => i.Name.ToLower().Contains(Search_tb.Text.ToLower()));
        }

        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (Templates_lv.SelectedItems.Count != 0)
            {
                b_Select.IsEnabled = true;
            }
            else
            {
                b_Select.IsEnabled = false;
            }
        }
    }
}
