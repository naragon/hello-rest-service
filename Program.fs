open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe

// Define the handler to return the greeting
let greetHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        let name = ctx.TryGetQueryStringValue "name" |> Option.defaultValue "World"
        let greeting = sprintf "Hello, %s!" name
        text greeting next ctx
        // ctx.Response.WriteAsync(greeting) |> Task.FromResult ctx

// Define the web app
let webApp =
    choose [
        GET >=> route "/greet" >=> greetHandler
        setStatusCode 404 >=> text "Not Found"
    ]

// Configure the app
let configureApp (app: IApplicationBuilder) =
    app.UseGiraffe webApp

// Configure services
let configureServices (services: IServiceCollection) =
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
                |> ignore)
        .Build()
        .Run()
    0
