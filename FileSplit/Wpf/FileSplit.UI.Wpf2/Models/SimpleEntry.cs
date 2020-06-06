using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileSplit.UI.Wpf2.Models
{
    class SimpleEntry : ViewModelBase
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        public string Label { get; set; } = "File";

    }
}
