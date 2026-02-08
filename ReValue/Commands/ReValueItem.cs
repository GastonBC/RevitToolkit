using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Utilities;

namespace ReValue.Commands
{
    public class ReValueItem : INotifyPropertyChanged
    {
        public ElementId Eid { get; set; }
        public string OldValue { get; set; }

        private string _newValue;
        public string NewValue
        {
            get => _newValue;
            set { _newValue = value; OnPropertyChanged(nameof(NewValue)); }
        }

        private bool _final;
        public bool Final
        {
            get => _final;
            set { _final = value; OnPropertyChanged(nameof(Final)); }
        }

        public string Tooltip { get; set; }

        public void FormatValue(string fromPattern, string toPattern)
        {
            if (Final) return;

            try
            {
                toPattern = toPattern ?? "";
                if (!string.IsNullOrEmpty(fromPattern))
                {
                    if (fromPattern.Contains("{") || fromPattern.Contains("}"))
                    {
                        this.NewValue = Utils.ReFormatString(this.OldValue, fromPattern, toPattern);
                        this.Tooltip = $"{fromPattern} --> {toPattern}";
                    }
                    else
                    {
                        // Simple regex replace for non-tagged strings
                        this.NewValue = Regex.Replace(this.OldValue, fromPattern, toPattern);
                    }
                }
                else
                {
                    this.Tooltip = "No Conversion Specified";
                    this.NewValue = "";
                }
            }
            catch (Exception ex)
            {
                this.NewValue = "";
                this.Tooltip = ex.Message;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}