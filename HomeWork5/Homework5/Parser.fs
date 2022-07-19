module Homework5.Parser
open System

let parseToOperation operation =
    match operation with 
    | "+" -> Operation.Plus |> Ok
    | "-" -> Operation.Minus |> Ok
    | "*" -> Operation.Multiply |> Ok
    | "/" -> Operation.Divide |> Ok
    | _ -> Error ErrorType.InvalidOperation
let tryParseToDecimal s = Exception

let parseArguments (args:string[]) = Exception