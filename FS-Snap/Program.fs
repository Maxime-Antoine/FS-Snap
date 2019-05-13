
open Snap

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


    let cards = generateDecks n

    assert (cards.Length = n*52)

    0 // return an integer exit code
