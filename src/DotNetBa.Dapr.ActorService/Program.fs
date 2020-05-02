namespace DotNetBa.Dapr.ActorService

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Dapr.Actors.AspNetCore

module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder
                    .UseStartup<Startup>()
                    .UseActors(fun actorRuntime -> actorRuntime.RegisterActor<FancyActor>())
                    .UseUrls(sprintf "http://localhost:%i" 5003)
                |> ignore
            )

    [<EntryPoint>]
    let main args =
        CreateHostBuilder(args).Build().Run()

        exitCode
