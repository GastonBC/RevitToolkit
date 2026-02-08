using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Toolbox.TemplateReplace
{
    public class ViewPortItem : INotifyPropertyChanged
    {
        public View ViewTemplate { get; set; }

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


    public partial class MainWn : Window
    {
        private UIDocument uidoc = null;
        private Document doc = null;
        private readonly ObservableCollection<ViewPortItem> TempObsCol = new ObservableCollection<ViewPortItem>();
        readonly List<View> TemplateCollector = null;

        public MainWn(UIDocument uid)
        {
            InitializeComponent();

            uidoc = uid;
            doc = uidoc.Document;

            TemplateCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .Cast<View>()
                .Where(v => v.IsTemplate)
                .ToList();


            foreach (View Template in TemplateCollector)
            {
                ViewPortItem addition = new ViewPortItem
                {
                    Checked = false,
                    ViewTemplate = Template
                };

                TempObsCol.Add(addition);

            }

            TempObsCol = new ObservableCollection<ViewPortItem>(TempObsCol.OrderBy(w => w.ViewTemplate.Name));

            TemplatesLV.ItemsSource = TempObsCol;
        }

        private void search_txt_changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TemplatesLV.ItemsSource = TempObsCol.Where(c => c.ViewTemplate.Name.ToLower().Contains(Search_tb.Text.ToLower()));
        }

        private void onCheckBoxCheck(object sender, RoutedEventArgs e)
        {
            SelectionCheckChange();
            // deselect single item if a checkbox is checked or it will change your selection
            // as well
            if (TemplatesLV.SelectedItems.Count == 1)
            {
                TemplatesLV.SelectedItem = null;
            }

            else
            {
                foreach (object ListObj in TemplatesLV.SelectedItems)
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
            if (TemplatesLV.SelectedItems.Count == 1)
            {
                TemplatesLV.SelectedItem = null;
            }

            else
            {
                foreach (object ListObj in TemplatesLV.SelectedItems)
                {
                    ViewPortItem ListItem = ListObj as ViewPortItem;
                    ListItem.Checked = false;
                }
            }
        }

        private void SelCats_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            List<View> CheckedTemplates = new List<View>();

            foreach (ViewPortItem VPItem in TempObsCol)
            {
                if (VPItem.Checked)
                {
                    CheckedTemplates.Add(VPItem.ViewTemplate);
                }
            }

            SecondWn WorksetWindow = new SecondWn(uidoc, CheckedTemplates, TemplateCollector);
            WorksetWindow.ShowDialog();
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            TemplatesLV.SelectedItems.Clear();
        }

        private void SelAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ViewPortItem CatElem in TempObsCol)
            {
                CatElem.Checked = true;
            }
        }

        private void DeselAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ViewPortItem CategoryElem in TempObsCol)
            {
                CategoryElem.Checked = false;
            }
        }

        private void SelectionCheckChange()
        {
            // this is another check
            // if there is one or more items checked, enable the button

            IEnumerable<ViewPortItem> checkedCats = TempObsCol.Where(item => item.Checked == true);

            if (checkedCats.Count() == 0) // If no categories selected
            {
                SelTemplates.IsEnabled = false;
            }
            else
            {
                SelTemplates.IsEnabled = true;
            }
        }
    }
}
