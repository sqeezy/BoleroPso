module BoleroPso.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client
open PSO

type Model =
    {
        SelectedFunction : OptimizationProblem
    }

let xSquaredProblem =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    p1*p1
  {
    Description = "x²"
    Func = xSquared
    InputRange = (0., 5.)
    MaxVelocity=0.1
    Dimension = 1
  }

let initModel =
    {
        SelectedFunction = xSquaredProblem
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
        p [] [text model.SelectedFunction.Description]
    ]

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
#if DEBUG
        |> Program.withHotReload
#endif
