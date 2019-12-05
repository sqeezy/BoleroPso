module BoleroPso.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client
open BoleroPso.Client.ExampleProblems
open PSO

type Model =
    {
        SelectedFunction : OptimizationProblem
        Functions : OptimizationProblem list
    }

let initModel =
    {
        Functions = exampleProblems
        SelectedFunction = List.head exampleProblems
    }

type Message =
    | Ping

let update message model =
    match message with
    | Ping -> model

let tableRow problem =
    tr [] [
        td [] [text problem.Description]
        td [] [textf "%i" problem.Dimension]
        td [] [textf "%A" problem.InputRange]
        td [] [textf "%f" problem.MaxVelocity]
    ]

let tableLayout problems=
    table [attr.``class`` "table"] [
        thead [] [
            th [] [text "Description"]
            th [] [text "Dimension"]
            th [] [text "Input Range"]
            th [] [text "Maximum Velocity"]
        ]
        tbody [] [
            forEach problems <| tableRow
        ]
    ]

let codeCol o =
    div [attr.``class`` "column"] [
        code [][textf "%A" o]
    ]

let columnLayout problems =
    forEach problems <| fun p ->
        div [attr.``class`` "columns"] [
                p.Description |> codeCol
                p.Dimension |> codeCol
                p.Func |> codeCol
                p.InputRange |> codeCol
                p.MaxVelocity |> codeCol
            ]

let view model dispatch =
    div[] [
        h1 [] [text "Particle Swarm Optimizer"]
        hr []
        h2 [] [text "Column Layout"]
        columnLayout model.Functions
        hr []
        h2 [] [text "Table Layout"]
        tableLayout model.Functions
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
#if DEBUG
        |> Program.withHotReload
#endif
