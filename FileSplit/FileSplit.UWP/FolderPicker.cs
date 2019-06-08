using System;
using FileSplit.Core;
using System.Threading.Tasks;
using Xamarin.Forms;
using FileSplit.UWP;

[assembly: Dependency(typeof(FolderPicker))]
namespace FileSplit.UWP
{
    public class FolderPicker : IFolderPickService
    {
        public async Task<IFolderPicked> PickFolder()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                return new FolderPicked(folder);
            }
            else
            {
                return new FolderPicked(false);
            }
        }
    }
}
