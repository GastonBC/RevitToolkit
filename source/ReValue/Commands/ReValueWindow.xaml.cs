using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ReValue.Commands
{
    public partial class ReValueWindow : Window
    {
        private Document _doc;
        private List<Element> _targetElements;
        private ObservableCollection<ReValueItem> _revalueItems;

        public ReValueWindow(Document doc, List<Element> elements)
        {
            InitializeComponent();
            _doc = doc;
            _targetElements = elements;
            _revalueItems = new ObservableCollection<ReValueItem>();
            preview_dg.ItemsSource = _revalueItems;

            SetupParameters();
        }

        private void SetupParameters()
        {
            HashSet<string> uniqueParams = new HashSet<string>();

            foreach (Element el in _targetElements)
            {
                foreach (Parameter p in el.Parameters)
                {
                    if (!p.IsReadOnly && p.StorageType == StorageType.String)
                    {
                        uniqueParams.Add(p.Definition.Name);
                    }
                }
            }

            List<string> allParams = new List<string> { "Name", "Family: Name" };
            allParams.AddRange(uniqueParams.OrderBy(x => x));

            params_cb.ItemsSource = allParams;
            params_cb.SelectedIndex = 0;
        }

        private void OnParamChange(object sender, SelectionChangedEventArgs e)
        {
            RefreshPreview();
        }

        private void OnFormatChange(object sender, TextChangedEventArgs e)
        {
            string from = orig_format_tb.Text;
            string to = new_format_tb.Text;

            foreach (var item in _revalueItems)
            {
                item.FormatValue(from, to);
            }
        }

        private void RefreshPreview()
        {
            _revalueItems.Clear();
            string selected = params_cb.SelectedItem as string;
            if (string.IsNullOrEmpty(selected)) return;

            foreach (Element el in _targetElements)
            {
                string oldValue = "";

                if (selected == "Name")
                {
                    oldValue = el.Name;
                }
                else if (selected == "Family: Name")
                {
                    Element type = _doc.GetElement(el.GetTypeId());
                    if (type is FamilySymbol symbol)
                    {
                        oldValue = symbol.Family.Name;
                    }
                }
                else
                {
                    Parameter p = el.LookupParameter(selected);
                    if (p != null) oldValue = p.AsString();
                }

                var item = new ReValueItem { Eid = el.Id, OldValue = oldValue };
                item.FormatValue(orig_format_tb.Text, new_format_tb.Text);
                _revalueItems.Add(item);
            }
        }

        private void OnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (preview_dg.SelectedItems.Count == 1 && string.IsNullOrEmpty(new_format_tb.Text))
            {
                var selected = (ReValueItem)preview_dg.SelectedItem;
                orig_format_tb.Text = selected.OldValue;
            }
        }

        private void MarkAsFinal(object sender, RoutedEventArgs e)
        {
            foreach (ReValueItem item in preview_dg.SelectedItems)
            {
                item.Final = true;
            }
        }

        private void ApplyNewValues(object sender, RoutedEventArgs e)
        {
            string selectedParam = params_cb.SelectedItem as string;

            using (Transaction t = new Transaction(_doc, $"ReValue {selectedParam}"))
            {
                t.Start();
                try
                {
                    foreach (var item in _revalueItems)
                    {
                        if (string.IsNullOrEmpty(item.NewValue)) continue;

                        Element el = _doc.GetElement(item.Eid);
                        if (selectedParam == "Name")
                        {
                            el.Name = item.NewValue;
                        }
                        else if (selectedParam == "Family: Name")
                        {
                            Element type = _doc.GetElement(el.GetTypeId());
                            if (type is FamilySymbol symbol) symbol.Family.Name = item.NewValue;
                        }
                        else
                        {
                            Parameter p = el.LookupParameter(selectedParam);
                            p?.Set(item.NewValue);
                        }
                    }
                    t.Commit();
                    this.Close();
                }
                catch (Exception ex)
                {
                    t.RollBack();
                    TaskDialog.Show("Error", ex.Message);
                }
            }
        }
    }
}
