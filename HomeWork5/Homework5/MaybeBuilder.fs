module Homework5.MaybeBuilder

open System

type MaybeBuilder() =
    member this.Bind() = Exception
    member this.Return() = Exception
let maybe = MaybeBuilder()

