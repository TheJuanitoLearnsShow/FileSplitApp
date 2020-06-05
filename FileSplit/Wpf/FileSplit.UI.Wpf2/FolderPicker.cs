using System;
using FileSplit.Core;
using System.Threading.Tasks;
using FileSplit.Core.Types;
using System.Windows.Forms;

namespace FileSplit.UI.Wpf2
{
    public class FolderPicker : IFolderPickService
    {
        public async Task<IFolderPicked> PickFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (DialogResult.OK == result)
                {
                    return new FolderPicked(dialog.SelectedPath);
                }else
                {

                    return new FolderPicked(false);
                }
            }
        }
    }
}
