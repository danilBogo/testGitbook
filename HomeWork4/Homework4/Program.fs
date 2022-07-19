namespace Homework4

open Homework4
open Homework4.Calculator

module Program =     
    [<EntryPoint>]
    let main argv =
        let result = Calculate 0. Operation.Unknown 0.
        printfn $"{result}"
        0 