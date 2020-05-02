namespace DotNetBa.Dapr.ActorService

open System
open Dapr.Actors;
open Dapr.Actors.Runtime;
open DotNetBa.Dapr.Common
open System.Threading.Tasks;

type FancyActor(actorService: ActorService, actorId: ActorId) =
    inherit Actor(actorService, actorId)

    member _.StateManager = base.StateManager

    override this.OnActivateAsync() : Task =
        let message = this.Id.ToString() |> sprintf "Actor %s is starting"
        Console.WriteLine(message)
        Task.CompletedTask

    interface IFancyActor with
        member this.PerformComplexCalculation(state: Models.FancyActorState): Task<Models.FancyActorState> =
            async {
                return state
            } |> Async.StartAsTask