using FileSplit.Core.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileSplit.UI.Wpf2
{
    class FolderPicked : IFolderPicked
    {
        private readonly bool _picked;
        private readonly string _folder;

        public FolderPicked(bool picked)
        {
            _picked = picked;
        }
        public FolderPicked(string folder)
        {
            _folder = folder;
            _picked = true;
        }

        public async Task<Stream> CreateFile(string fileName)
        {
            var newFileName = Path.Combine(_folder, fileName);
            var newfIle = File.Create(newFileName);
            return newfIle;
        }

        public bool FolderWasPicked()
        {
            return _picked;
        }

        public string GetFolderPath()
        {
            return _folder;
        }
        public async Task OpenInExplorer()
        {
            Process.Start("explorer.exe", _folder);  
        }
    }
}
