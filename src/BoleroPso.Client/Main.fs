module BoleroPso.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client
open BoleroPso.Client.ExampleProblems
open PSO
open PSO.SequentialOptimizer

type Model =
    {
        SelectedFunction : OptimizationProblem
        Functions : OptimizationProblem list
        LastSolution : Solution option
    }

let initModel =
    {
        Functions = exampleProblems
        SelectedFunction = List.head exampleProblems
        LastSolution = None
    }

type Message =
    | SelectProblem of OptimizationProblem
    | StartOptimization

let log s1 s2 = ()

let testProblem problem =
    let config = {MaxIterations=1000}
    let solution = solve problem config log
    solution

let update message model =
    match message with
    | SelectProblem p -> {model with SelectedFunction = p}
    | StartOptimization ->
        let solution = testProblem model.SelectedFunction
        {model with LastSolution = Some solution}

let problemSelector model problem dispatch =
    label [attr.``class`` "radio"][
        input [
            attr.``type`` "radio"
            attr.``name`` "answer"
            on.click (fun _ -> dispatch (SelectProblem problem))
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
        button [on.click (fun _ -> dispatch StartOptimization)] [
            textf "Start Optimization %s" model.SelectedFunction.Description
        ]
        hr []
        h2 [] [text "Last Solution"]
        cond model.LastSolution.IsSome <| function
        | false -> empty
        | true -> 
            let (parameters, fitnesse) = model.LastSolution.Value
            label [] [textf "Parameters: %A -> Fitnesse: %f" parameters fitnesse]
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
#if DEBUG
        |> Program.withHotReload
#endif
