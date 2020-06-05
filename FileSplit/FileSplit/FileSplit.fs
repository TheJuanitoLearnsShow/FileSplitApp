// Copyright 2018 Fabulous contributors. See LICENSE.md for license.
namespace FileSplit

open System.Diagnostics
open Xamarin.Forms
open FileSplit.Core
open Plugin.FilePicker
open Plugin.FilePicker.Abstractions
open Fabulous.XamarinForms.LiveUpdate
open FileSplit.Core.Types
open Fabulous
open Fabulous.XamarinForms
open FileSplit.Types
open FileSplit.Commands.IoCommands
open FileSplit.Commands.SplitCommands

module App = 
    

    let initModel = { InputFilePath =""; OutputFilePath =""; FileCount = 0; FilesCreated= Seq.empty; InputFileData = None; FolderPicked = None}

    let init () = initModel , Cmd.none

                

    let update msg model =
        match msg with
        | InputFilePath s -> { model with InputFilePath = s }, Cmd.none
        | OutputFilePath s -> { model with OutputFilePath = s }, Cmd.none
        | FileCountUpdate i -> model, Cmd.none
        | DoSplit -> model, splitCmd model 
        | SplitCompleted r -> 
            match r with
            | Ok files ->
                { model with FilesCreated = files }, Cmd.none
            | Error err ->
                { model with FilesCreated = [err ] }, Cmd.none
        | BrowseInputFile ->
            model, getFileCmd
        | BrowseInputFileCompleted f -> 
            match f with
            | Some filedata ->
                { model with InputFileData = f; InputFilePath = filedata.FileName; OutputFilePath = filedata.FileName }, Cmd.none
            | None ->
                { model with InputFileData = f; InputFilePath = "" }, Cmd.none
        | BrowseOutputFolder ->
            model, getFolderCmd
        | BrowseOutputFolderCompleted f -> 
            match f with
            | Some filedata ->
                { model with FolderPicked = f }, Cmd.none
            | None ->
                { model with FolderPicked = f }, Cmd.none
        | OpenInExplorer ->
            model, openFolderCmd model
        | DoNothing ->
            model, Cmd.none

    let view (model: Model) dispatch =
        let files = 
            model.FilesCreated
            |> Seq.map (fun f ->
                View.Label(text = f, horizontalOptions = LayoutOptions.Center)
            )
            |> Seq.toList
        let childrenElems = 
            [ 
                View.StackLayout(padding = Thickness 20.0, orientation = StackOrientation.Horizontal,
                    children =
                        [
                            View.Label(text = "Input File Path", horizontalOptions = LayoutOptions.Center)
                            View.Entry(text = model.InputFilePath, 
                                //textChanged = (fun e -> e.NewTextValue |> InputFilePath |> dispatch)  , 
                                completed = (fun text -> text |> InputFilePath |> dispatch),
                                horizontalOptions = LayoutOptions.Center, 
                                horizontalTextAlignment=TextAlignment.Center)
        
                            View.Button(text = "Browse for Input", command = (fun () -> dispatch BrowseInputFile), horizontalOptions = LayoutOptions.Center)
                        ]
                )

                View.Label(text = "Output File Path", horizontalOptions = LayoutOptions.Center)
                View.Entry(text = model.OutputFilePath, 
                        //textChanged = (fun e -> e.NewTextValue |> OutputFilePath |> dispatch), 
                        completed = (fun text -> text |> OutputFilePath |> dispatch),
                        horizontalOptions = LayoutOptions.Center, 
                        horizontalTextAlignment=TextAlignment.Center)
        
                View.Button(text = "Browse for Output", command = (fun () -> dispatch BrowseOutputFolder), horizontalOptions = LayoutOptions.Center)

                View.Button(text = "Do Split", command = (fun () -> dispatch DoSplit), horizontalOptions = LayoutOptions.Center)
                
                View.Button(text = "Open Folder with Split Files", command = (fun () -> dispatch OpenInExplorer), horizontalOptions = LayoutOptions.Center)
        
        
            ] |> List.append files
        View.ContentPage(
          content = View.StackLayout(padding = Thickness 20.0, verticalOptions = LayoutOptions.Center,
            children = childrenElems
                
            
            ))

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()
    
    let runner =
        Program.mkProgram App.init App.update App.view
        |> Program.withConsoleTrace
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/tools.html for further  instructions.
    //
    do runner.EnableLiveUpdate ()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/models.html for further  instructions.
#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


