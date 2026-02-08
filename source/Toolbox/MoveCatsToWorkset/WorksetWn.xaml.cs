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
using Utilities;

namespace Toolbox.MoveCatsToWorkset
{

    public partial class WorksetWn : Window
    {

        UIDocument uidoc = null;
        Document doc = null;

        List<Category> Categories = new List<Category>();
        List<Workset> Wksets = new List<Workset>();
        public WorksetWn(UIDocument uid,
                            List<Category> CatCollector,
                            List<Workset> WkstCollector)
        {
            InitializeComponent();
            uidoc = uid;
            doc = uidoc.Document;

            Categories = CatCollector;
            Wksets = WkstCollector;

            Workset_lv.ItemsSource = Wksets;

        }

        private void b_Select_Click(object sender, RoutedEventArgs e)
        {
            Workset WksetChosen = Workset_lv.SelectedItem as Workset;

            using (Transaction t = new Transaction(doc, "Category to Workset"))
            {
                int n = 0;

                t.Start();
                foreach (Category cat in Categories)
                {
                    List<Element> ElemsByCat = new FilteredElementCollector(doc).OfCategoryId(cat.Id).ToElements().ToList();

                    foreach (Element elem in ElemsByCat)
                    {
                        if (doc.GetWorksetId(elem.Id) != WksetChosen.Id)
                        {
                            try
                            {
                                Parameter param = elem.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                                param.Set(WksetChosen.Id.IntegerValue);
                                n++;
                            }
                            catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                            {
                            }
                        }
                    }

                }
                t.Commit();

                Utils.SimpleDialog("Moved " + n + " elements to " + WksetChosen.Name, "");
            }

            Close();
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            Workset_lv.SelectedItems.Clear();
        }

        private void search_txt_changed(object sender, TextChangedEventArgs e)
        {
            Workset_lv.ItemsSource = Wksets.Where(i => i.Name.ToLower().Contains(Search_tb.Text.ToLower()));
        }

        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if(Workset_lv.SelectedItems.Count != 0)
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