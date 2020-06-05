namespace FileSplit.Commands

open FileSplit.Core
open Fabulous

module SplitCommands =
    open FileSplit.Types

    let splitCmd model = 
        match model.InputFileData, model.FolderPicked with
        | Some d, Some folderPicked->
            let inputStream = d.GetStream()
            let outFile = model.OutputFilePath
            async { let! result = Splitter.SplitFile (fun i -> ()) inputStream folderPicked outFile 5 
                    return SplitCompleted result }
            |> Cmd.ofAsyncMsg
        | _,_ ->
            DoNothing |> Cmd.ofMsg


