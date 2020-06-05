namespace FileSplit.Types

open FileSplit.Core.Types
open Plugin.FilePicker.Abstractions


type Model = 
    { InputFilePath : string
      OutputFilePath : string
      InputFileData : FileData option
      FileCount : int
      FilesCreated : string seq
      FolderPicked: IFolderPicked option}

type Msg = 
    | InputFilePath of string
    | OutputFilePath  of string
    | DoSplit
    | FileCountUpdate of int
    | SplitCompleted of Result<string seq, string>
    | BrowseInputFile 
    | BrowseInputFileCompleted of FileData option
    | BrowseOutputFolder 
    | BrowseOutputFolderCompleted of IFolderPicked option
    | OpenInExplorer
    | DoNothing