namespace FileSplit.Core

open System.IO
open System.Collections.Generic
open System.Threading.Tasks


type IFolderPicked =
    abstract member FolderWasPicked : unit -> bool
    abstract member GetFolderPath : unit -> string
    abstract member CreateFile : string -> Task<Stream> 
    abstract member OpenInExplorer : unit -> Task
    
type IFolderPickService =
    abstract member PickFolder : unit -> Task<IFolderPicked>

module Splitter =
    let SplitFile currFileNotifier (inputStream:Stream) (folderPicked:IFolderPicked) (outputFileName:string) (maxLinesPerFile:int)  =
        async {
            let filesCreated = List<string>()
            let outputFolder = folderPicked.GetFolderPath() //C:\Users\<user>\AppData\Local\Packages\f33bd753-7cb6-4177-bf50-b993ed286c3b_dcy57zvw0vndm\LocalState
            let! outStream = folderPicked.CreateFile outputFileName |> Async.AwaitTask
            let outputFilepath = Path.Combine( outputFolder, outputFileName)
            let mutable fileWriter = new StreamWriter(outStream)
            filesCreated.Add(outputFilepath)
            use fileReader = new StreamReader(inputStream)
            let mutable currLineNumber = 0
            let mutable currFileNumber = 1
            try 
                let mutable currLine = fileReader.ReadLine()
                while currLine |> isNull |> not do
                    fileWriter.WriteLine(currLine)
                    currLineNumber <- currLineNumber + 1
                    if (currLineNumber >= maxLinesPerFile) then
                        fileWriter.Close()
                        fileWriter.Dispose()
                        currFileNumber <- currFileNumber + 1
                        let newFileName = Path.GetFileNameWithoutExtension(outputFilepath) + "-" + currFileNumber.ToString() + Path.GetExtension(outputFilepath)
                        let! newStream = folderPicked.CreateFile newFileName |> Async.AwaitTask
                        let outputFilepath = Path.Combine( outputFolder, newFileName)
                        fileWriter <- new StreamWriter(newStream)
                        filesCreated.Add(outputFilepath)
                        currFileNotifier  currFileNumber
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
