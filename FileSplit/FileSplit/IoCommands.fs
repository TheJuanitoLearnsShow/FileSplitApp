namespace FileSplit.Commands

open Plugin.FilePicker

open Xamarin.Forms
open FileSplit.Core.Types
open Fabulous

module IoCommands =

    open Fabulous
    open FileSplit.Types
    open FileSplit.Core
    open FileSplit.Core.Types


    let getFileCmd =
        async {
            try
                let! fileData = (CrossFilePicker.Current.PickFile()) |> Async.AwaitTask
                if (fileData |> isNull) then
                    return None |> BrowseInputFileCompleted
                else 

                    let fileName = fileData.FileName;

                    System.Console.WriteLine("File name chosen: " + fileName);
                    return  fileData |> Some |> BrowseInputFileCompleted
            
            with ex ->
                System.Console.WriteLine("Exception choosing file: " + ex.ToString()) ;
                return None |> BrowseInputFileCompleted
        } |> Cmd.ofAsyncMsg
    
    let getFolderCmd =
        async {
            try
            
                let! folderPicked = DependencyService.Get<IFolderPickService>().PickFolder() |> Async.AwaitTask
                if (folderPicked.FolderWasPicked()) then
                    return  folderPicked |> Some |> BrowseOutputFolderCompleted
                else 
                    return None |> BrowseOutputFolderCompleted
                
            with ex ->
                System.Console.WriteLine("Exception choosing folder: " + ex.ToString()) ;
                return None |> BrowseInputFileCompleted
        } |> Cmd.ofAsyncMsg

    let openFolderCmd model =
        async {
            try
            
                match model.FolderPicked with
                | Some folderPicked->
                    do! folderPicked.OpenInExplorer()  |> Async.AwaitTask
                    return DoNothing
                | None ->
                    return DoNothing
                
            with ex ->
                System.Console.WriteLine("Exception choosing folder: " + ex.ToString()) ;
                return DoNothing
        } |> Cmd.ofAsyncMsg