using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Toolbox.MoveCatsToWorkset
{

    public class ViewPortItem : INotifyPropertyChanged
    {
        public Category Category { get; set; }

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked == value)
                    return;
                _Checked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Checked"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class CatToWkst : Window
    {
        private UIDocument uidoc = null;
        private Document doc = null;
        private readonly ObservableCollection<ViewPortItem> CatCollection = new ObservableCollection<ViewPortItem>();
        readonly List<Workset> WkstCollector = null;

        public CatToWkst(UIDocument uid)
        {
            InitializeComponent();

            uidoc = uid;
            doc = uidoc.Document;
            

            Categories CatSettings = doc.Settings.Categories;

            WkstCollector = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .OrderBy(w => w.Name)
                .ToList();


            foreach (Category CatInstance in CatSettings)
            {
                ViewPortItem addition = new ViewPortItem
                {
                    Checked = false,
                    Category = CatInstance
                };

                CatCollection.Add(addition);
                
            }

            CatCollection = new ObservableCollection<ViewPortItem>(CatCollection.OrderBy(w => w.Category.Name));

            CategoryLV.ItemsSource = CatCollection;
        }

        private void search_txt_changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CategoryLV.ItemsSource = CatCollection.Where(c => c.Category.Name.ToLower().Contains(Search_tb.Text.ToLower()) );
        }

        private void onCheckBoxCheck(object sender, RoutedEventArgs e)
        {
            SelectionCheckChange();
            // deselect single item if a checkbox is checked or it will change your selection
            // as well
            if (CategoryLV.SelectedItems.Count == 1)
            {
                CategoryLV.SelectedItem = null;
            }

            else
            {
                foreach (object ListObj in CategoryLV.SelectedItems)
                {
                    ViewPortItem ListItem = ListObj as ViewPortItem;
                    ListItem.Checked = true;
                }
            }

        }

        private void onCheckBoxUncheck(object sender, RoutedEventArgs e)
        {
            SelectionCheckChange();
            // deselect single item if a checkbox is checked or it will change your selection
            // as well
            if (CategoryLV.SelectedItems.Count == 1)
            {
                CategoryLV.SelectedItem = null;
            }

            else
            {
                foreach (object ListObj in CategoryLV.SelectedItems)
                {
                    ViewPortItem ListItem = ListObj as ViewPortItem;
                    ListItem.Checked = false;
                }
            }
        }

        private void SelCats_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            List<Category> CheckedCatList = new List<Category>();

            foreach (ViewPortItem VPItem in CatCollection)
            {
                if (VPItem.Checked)
                {
                    CheckedCatList.Add(VPItem.Category);
                }
            }

            WorksetWn WorksetWindow = new WorksetWn(uidoc, CheckedCatList, WkstCollector);
            WorksetWindow.ShowDialog();
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            CategoryLV.SelectedItems.Clear();
        }

        private void SelAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ViewPortItem CatElem in CatCollection)
            {
                CatElem.Checked = true;
            }
        }

        private void DeselAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ViewPortItem CategoryElem in CatCollection)
            {
                CategoryElem.Checked = false;
            }
        }

        private void SelectionCheckChange()
        {
            // this is another check
            // if there is one or more items checked, enable the button

            IEnumerable<ViewPortItem> checkedCats = CatCollection.Where(item => item.Checked == true);

            if (checkedCats.Count() == 0) // If no categories selected
            {
                SelSheets.IsEnabled = false;
            }
            else
            {
                SelSheets.IsEnabled = true;
            }
        }
    }
}
