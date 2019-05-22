
open Snap
open System

[<EntryPoint>]
let main argv =
    let n = 
        match selectNbOfPacks () with
        | Some n when n > 0 -> n
        | _ -> 1

    let matchingConditionNo = 
        match selectMatchingCondition () with
        | Some n when n >= 1 && n <= 3 -> n
        | _ -> 1

    printfn "n is %i and matchingConditionNo is %i" n matchingConditionNo

    let cards = generateDecks n |> List.toArray |> FisherYatesShuffle |> Seq.toList

    assert (cards.Length = n * 52)

    let snapFn card1 card2 =
        match matchingConditionNo with
        | 1 -> card1.Face = card2.Face
        | 2 -> card1.Suit = card2.Suit
        | _ -> card1 = card2

    let result = play snapFn cards [] [] []

    Console.WriteLine(result.ToString())
       
    0 // return an integer exit code
