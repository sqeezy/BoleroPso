module BoleroPso.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client
open BoleroPso.Client.ExampleProblems

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

let view model dispatch =
    div[] [
        h1 [] [text "Particle Swarm Optimizer"]
        hr []
        forEach model.Functions <| fun f ->
            p [] [text f.Description]
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
#if DEBUG
        |> Program.withHotReload
#endif
