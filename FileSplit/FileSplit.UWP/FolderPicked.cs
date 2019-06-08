using Windows.Storage;
using System.IO;
using FileSplit.Core;
using System.Threading.Tasks;
using System;
using Windows.System;

namespace FileSplit.UWP
{
    public class FolderPicked : IFolderPicked
    {
        private  readonly bool _picked;
        private readonly Windows.Storage.StorageFolder _folder;

        public FolderPicked(bool picked)
        {
            _picked = picked;
        }
        public FolderPicked(StorageFolder folder)
        {
            _folder = folder;
            _picked = true;
        }

        public async Task<Stream> CreateFile(string fileName)
        {

            Windows.Storage.AccessCache.StorageApplicationPermissions.
            FutureAccessList.AddOrReplace("PickedFolderToken", _folder);
            Windows.Storage.StorageFile sampleFile =
                await _folder.CreateFileAsync(fileName,
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);

            return await sampleFile.OpenStreamForWriteAsync();
        }

        public bool FolderWasPicked()
        {
            return _picked;
        }

        public string GetFolderPath()
        {
            return _folder.Name;
        }
        public async Task OpenInExplorer()
        {
            await Launcher.LaunchFolderAsync(_folder);
        }
    }
}
