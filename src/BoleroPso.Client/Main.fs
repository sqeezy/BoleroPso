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
    | SelectProblem of OptimizationProblem

let update message model =
    match message with
    | Ping -> model
    | SelectProblem p -> {model with SelectedFunction = p}

let problemSelector model problem dispatch =
    label [attr.``class`` "radio"][
        input [
            attr.``type`` "radio"
            attr.``name`` "answer"
            bind.checked (problem.Description=model.SelectedFunction.Description) (fun c -> dispatch (SelectProblem problem))
        ]
    ]

let tableRow model dispatch problem =
    tr [] [
        td [] [problemSelector model problem dispatch]
        td [] [text problem.Description]
        td [] [textf "%i" problem.Dimension]
        td [] [textf "%A" problem.InputRange]
        td [] [textf "%f" problem.MaxVelocity]
    ]

let tableLayout model dispatch=
    table [attr.``class`` "table"] [
        thead [] [
            th [] [text "Selected"]
            th [] [text "Description"]
            th [] [text "Dimension"]
            th [] [text "Input Range"]
            th [] [text "Maximum Velocity"]
        ]
        tbody [] [
            forEach model.Functions <| (tableRow model dispatch)
        ]
    ]


let view model dispatch =
    div[] [
        h1 [] [text "Particle Swarm Optimizer"]
        hr []
        h2 [] [text "Predifined Problems"]
        tableLayout model dispatch
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
#if DEBUG
        |> Program.withHotReload
#endif
