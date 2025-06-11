using System;
using FileSplit.Core;
using System.Threading.Tasks;
using FileSplit.Core.Types;
using System.Windows.Forms;

namespace FileSplit.UI.Wpf2
{
    public class FolderPicker : IFolderPickService
    {
        public Task<IFolderPicked> PickFolder()
        {
            using var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            return Task.FromResult<IFolderPicked>(DialogResult.OK == result ? new FolderPicked(dialog.SelectedPath) : new FolderPicked(false));
        }
    }
}
