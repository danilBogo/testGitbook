module Giraffe.App
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) = 0

    member _.Configure (app : IApplicationBuilder)
                        (_ : IHostEnvironment)
                        (_ : ILoggerFactory) = 0
        
[<EntryPoint>]
let main args =
    0