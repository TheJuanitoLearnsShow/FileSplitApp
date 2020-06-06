using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileSplit.UI.Wpf2.Models
{
    class SplitForm : ViewModelBase
    {

        public FileEntry InputFileInfo
        {
            get => inputFileInfo; 
            set
            {
                if (inputFileInfo != null)
                {
                    inputFileInfo.PropertyChanged -= FileEntryChanged;
                }
                inputFileInfo = value;
                inputFileInfo.PropertyChanged += FileEntryChanged;
            }
        }

        private void FileEntryChanged(object sender, PropertyChangedEventArgs e)
        {
            EntriesSelected = !String.IsNullOrEmpty(inputFileInfo.SelectedFilePath) && !String.IsNullOrEmpty(outputFolderInfo.SelectedFilePath);
        }

        public FileEntry OutputFolderInfo { get => outputFolderInfo; set
            {
                if (outputFolderInfo != null)
                {
                    outputFolderInfo.PropertyChanged -= FileEntryChanged;
                }
                outputFolderInfo = value;
                outputFolderInfo.PropertyChanged += FileEntryChanged;
            }
        }
        private bool _entriesSelected;
        private FileEntry inputFileInfo;
        private FileEntry outputFolderInfo;

        public bool EntriesSelected
        {
            get { return _entriesSelected; }
            set { _entriesSelected = value; OnPropertyChanged(); }
        }

        private SimpleEntry _NewFilename;

        public SimpleEntry NewFilename
        {
            get { return _NewFilename; }
            set { _NewFilename = value; }
        } 
        private SimpleEntry _MaxLines;

        public SplitForm()
        {
            NewFilename = new SimpleEntry() { Label = "New Filename", Value = "NewSplitFile" };
            MaxLines = new SimpleEntry() { Label = "New Filename", Value = "1000" };
            InputFileInfo = new FileEntry() { IsFolder = false, Label = "File to Split", BrowseLabel = "Browse" };
            OutputFolderInfo = new FileEntry() { IsFolder = true, Label = "Output Folder", BrowseLabel = "Browse" };
        }

        public SimpleEntry MaxLines
        {
            get { return _MaxLines; }
            set { _MaxLines = value; }
        }


    }
}
