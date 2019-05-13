module Snap

open Microsoft.FSharp.Reflection
open System

type Suit = Diamond | Club | Spade | Heart

type Face = Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King | Ace

type Card = {
    Suit: Suit
    Face: Face
}

let isNb str = str |> Seq.forall Char.IsDigit

let selectNbOfPacks () = 
    printfn "Please enter the number of packs to use"
    let input = Console.ReadLine()
    match isNb input with
    | true -> int32 input |> Some
    | false -> None

let selectMatchingCondition () =
    printfn "Please select the matching condition to use:"
    printfn " 1) Face value"
    printfn " 2) Suit"
    printfn " 3) Both"
    let input = Console.ReadLine()
    match isNb input with
    | true -> int32 input |> Some
    | false -> None

let generateDecks n =
    let suitTypes = FSharpType.GetUnionCases typeof<Suit>
    let faceTypes = FSharpType.GetUnionCases typeof<Face>
    [for _ in [|1..n|] do
        for s in suitTypes do 
        for f in faceTypes do 
        yield {
           Suit = FSharpValue.MakeUnion(s, [||]) :?> Suit;  
           Face = FSharpValue.MakeUnion(f, [||]) :?> Face;
       }];
