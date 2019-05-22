module Snap

open Microsoft.FSharp.Reflection
open System

type Suit = Diamond | Club | Spade | Heart

type Face = Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King | Ace

type Card = {
    Suit: Suit
    Face: Face
}

type Result = Player1Wins | Player2Wins | Tie

type Players = Player1 | Player2

let isNonEmptyDigit char = Char.IsDigit char && not (Char.IsWhiteSpace char)
let isNb str = str |> Seq.forall isNonEmptyDigit && not (Seq.isEmpty str)

let selectNbOfPacks () = 
    printfn "Please enter the number of packs to use:  (default 1)"
    let input = Console.ReadLine()
    match isNb input with
    | true -> int32 input |> Some
    | false -> None

let selectMatchingCondition () =
    printfn "Please select the matching condition to use:"
    printfn " 1) Face value"
    printfn " 2) Suit"
    printfn " 3) Both  (default)"
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

let FisherYatesShuffle (array: array<'a>) = 
    let availableVals = Array.init array.Length (fun i -> (i, true))
    let random = new Random()
    let nextItem nLeft =
        let nItem = random.Next(0, nLeft)
        let index =
            availableVals
            |> Seq.filter (fun (_, f) -> f)
            |> Seq.item nItem
            |> fst
        availableVals.[index] <- (index, false)
        array.[index]
    seq {(array.Length) .. -1 .. 1}
    |> Seq.map (fun i -> nextItem i)

let chooseWinner (player1Cards: List<Card>) (player2Cards: List<Card>) =
    match (player1Cards.Length - player2Cards.Length) with
    | n when n > 0 -> Result.Player1Wins
    | n when n = 0 -> Result.Tie
    | _ -> Result.Player2Wins

let takeTopCard list =
    match list with
    | s::q -> (s, q)
    | _ -> failwith("Shouldn't happen")

let chooseRandomPlayer () = 
    match System.Random().Next(2) with
    | 0 -> Players.Player1
    | _ -> Players.Player2

let rec play snapFn (decks:List<Card>) (playedCards:List<Card>) (player1Cards:List<Card>) (player2Cards:List<Card>) =
    if (decks.IsEmpty) then
        chooseWinner player1Cards player2Cards
    else
        let currentCard, newDecks = takeTopCard decks
        if (not playedCards.IsEmpty && snapFn playedCards.Head currentCard) then
            let choosenPlayer = chooseRandomPlayer ()
            let newPlayer1Cards = match choosenPlayer with
                                  | Players.Player1 -> currentCard::playedCards@player1Cards
                                  | Players.Player2 -> player1Cards
            let newPlayer2Cards = match choosenPlayer with
                                  | Players.Player1 -> player2Cards
                                  | Players.Player2 -> currentCard::playedCards@player2Cards

            play snapFn newDecks [] newPlayer1Cards newPlayer2Cards 
        else
            play snapFn newDecks (currentCard::playedCards) player1Cards player2Cards
