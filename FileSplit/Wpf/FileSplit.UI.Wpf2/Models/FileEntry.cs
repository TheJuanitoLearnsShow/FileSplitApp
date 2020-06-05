using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileSplit.UI.Wpf2.Models
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    class FileEntry : ViewModelBase
    {
        private string _selectedFile;

        public string SelectedFilePath
        {
            get { return _selectedFile; }
            set { _selectedFile = value; OnPropertyChanged(); }
        }
        public string Label { get; set; } = "File";
        public string BrowseLabel { get; set; } = "Browse";
        public bool IsFolder { get; set; } = false;

    }
}
