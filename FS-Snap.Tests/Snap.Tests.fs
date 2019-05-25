namespace FS_Snap.Tests

open Snap
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type FisherYatesShuffleTest () =

    [<TestMethod>]
    member this.ShufflingIsRandom () =
        let array = Array.init 25 id
        let shuffled = FisherYatesShuffle array
        let shuffled2 = FisherYatesShuffle array

        Assert.AreNotEqual(array, shuffled)
        Assert.AreNotEqual(shuffled, shuffled2)

[<TestClass>]
type ChooseWinnerTest () =

    [<TestMethod>]
    member this.WhenThereIsAWinner () =
        let player1Cards = generateDecks 2
        let player2Cards = generateDecks 3
        let winner = chooseWinner player1Cards player2Cards

        Assert.AreEqual(winner, Result.Player2Wins)

    [<TestMethod>]
    member this.WhenThereIsATie () =
        let player1Cards = generateDecks 2
        let player2Cards = generateDecks 2
        let winner = chooseWinner player1Cards player2Cards

        Assert.AreEqual(winner, Result.Tie)
