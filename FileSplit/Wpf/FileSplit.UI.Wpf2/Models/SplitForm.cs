using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileSplit.UI.Wpf2.Models
{
    class SplitForm : ViewModelBase
    {
        public FileEntry InputFileInfo { get; set; } 
        public FileEntry OutputFolderInfo { get; set; } 

    }
}
