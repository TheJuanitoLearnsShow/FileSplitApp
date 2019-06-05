// Copyright 2018 Fabulous contributors. See LICENSE.md for license.
namespace FileSplit

open System.Diagnostics
open Fabulous.Core
open Fabulous.DynamicViews
open Xamarin.Forms
open FileSplit.Core

module App = 
    type Model = 
      { InputFilePath : string
        OutputFilePath : string
        FileCount : int
        FilesCreated : string seq }

    type Msg = 
        | InputFilePath of string
        | OutputFilePath  of string
        | DoSplit
        | FileCountUpdate of int
        | SplitCompleted of Result<string seq, string>

    let initModel = { InputFilePath =""; OutputFilePath =""; FileCount = 0; FilesCreated= Seq.empty }

    let init () = initModel, Cmd.none

    let splitCmd inputFile outFile = 
        async { let! result = Splitter.SplitFile (fun i -> ()) inputFile outFile 5 
                return SplitCompleted result }
        |> Cmd.ofAsyncMsg

    let update msg model =
        match msg with
        | InputFilePath s -> { model with InputFilePath = s }, Cmd.none
        | OutputFilePath s -> { model with OutputFilePath = s }, Cmd.none
        | FileCountUpdate i -> model, Cmd.none
        | DoSplit -> model, splitCmd model.InputFilePath model.OutputFilePath
        | SplitCompleted r -> 
            match r with
            | Ok files ->
                { model with FilesCreated = files }, Cmd.none
            | Error err ->
                { model with FilesCreated = [err ] }, Cmd.none
                

    let view (model: Model) dispatch =
        let files = 
            model.FilesCreated
            |> Seq.map (fun f ->
                View.Label(text = f, horizontalOptions = LayoutOptions.Center)
            )
            |> Seq.toList
        let childrenElems = 
            [ 
                View.Label(text = "Input File Path", horizontalOptions = LayoutOptions.Center)
                View.Entry(text = model.InputFilePath, textChanged = (fun e -> e.NewTextValue |> InputFilePath |> dispatch)  , horizontalOptions = LayoutOptions.Center, widthRequest=200.0, horizontalTextAlignment=TextAlignment.Center)
        
                View.Label(text = "Output File Path", horizontalOptions = LayoutOptions.Center)
                View.Entry(text = model.OutputFilePath, textChanged = (fun e -> e.NewTextValue |> OutputFilePath |> dispatch), horizontalOptions = LayoutOptions.Center, widthRequest=200.0, horizontalTextAlignment=TextAlignment.Center)
        
                View.Button(text = "Do Split", command = (fun () -> dispatch DoSplit), horizontalOptions = LayoutOptions.Center)

        
        
            ] |> List.append files
        View.ContentPage(
          content = View.StackLayout(padding = 20.0, verticalOptions = LayoutOptions.Center,
            children = childrenElems
                
            
            ))

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> Program.runWithDynamicView app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/tools.html for further  instructions.
    //
    //do runner.EnableLiveUpdate()
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


