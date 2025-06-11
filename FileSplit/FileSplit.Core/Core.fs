namespace FileSplit.Core

open System.IO
open System.Collections.Generic
open System.Threading.Tasks
open FileSplit.Core.Types


module Splitter =
    let private createNewFile (folderPicked:IFolderPicked) outputFolder (outputFilepath:string) encodingFound currFileNumber =
        async {
            let newFileName = Path.GetFileNameWithoutExtension(outputFilepath) + "-" + currFileNumber.ToString() + Path.GetExtension(outputFilepath)
            let! newStream = folderPicked.CreateFile newFileName |> Async.AwaitTask
            let newFilepath = Path.Combine( outputFolder, newFileName)
            let fileWriter = new StreamWriter(newStream, encodingFound)
            return (fileWriter, newFilepath)
        }
        

    let SplitFile currFileNotifier (inputStream:Stream) (folderPicked:IFolderPicked) (outputFileName:string) (maxLinesPerFile:int)  =
        async {
            let filesCreated = List<string>()
            let outputFolder = folderPicked.GetFolderPath() 
            let outputFilepath = Path.Combine( outputFolder, outputFileName)
            use fileReader = new StreamReader(inputStream, true)
            let encodingFound = fileReader.CurrentEncoding
            let mutable fileWriter = null
            filesCreated.Add(outputFilepath)
            let mutable currLineNumber = 0
            let mutable currFileNumber = 1
            let createNewSplitFile = createNewFile folderPicked outputFolder outputFilepath encodingFound
            try 
                let mutable currLine = fileReader.ReadLine()
                while currLine |> isNull |> not do
                    match currLineNumber with
                    | 0 ->
                        let! (newStream,newFilepath) = createNewSplitFile currFileNumber
                        fileWriter <- newStream
                        filesCreated.Add(newFilepath)
                        currFileNotifier  currFileNumber
                    | _ ->  
                        fileWriter.WriteLine()
                    fileWriter.Write(currLine)
                    currLineNumber <- currLineNumber + 1
                    if (currLineNumber >= maxLinesPerFile) then
                        fileWriter.Close()
                        fileWriter.Dispose()
                        currFileNumber <- currFileNumber + 1
                        currLineNumber <- 0
                    currLine <- fileReader.ReadLine()
                fileWriter.Close()
                fileWriter.Dispose()
                fileReader.Close()
                fileReader.Dispose()
                return Result.Ok(filesCreated :> seq<_>)
            with exn ->
                fileWriter.Close()
                fileWriter.Dispose()
                fileReader.Close()
                fileReader.Dispose()
                return Result.Error(exn.ToString())
        }
    
    let SplitFileAsTask currFileNotifier (inputStream:Stream) (folderPicked:IFolderPicked) (outputFileName:string) (maxLinesPerFile:int) = 
        SplitFile currFileNotifier (inputStream) (folderPicked) (outputFileName) (maxLinesPerFile)
        |> Async.StartAsTask