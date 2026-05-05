namespace Dobutsu

open System

type GameOption = PlayerFirst | AIFirst | Exit | TwoPlayers

module GameOption =
  let rec take () =
    printfn "Dobutsu Shogi"
    printfn "1. Player First"
    printfn "2. AI First"
    printfn "3. Two Players"
    printfn "4. Exit"
    Console.Write "> "
    match Console.ReadLine () with
    | "1" -> PlayerFirst
    | "2" -> AIFirst
    | "3" -> TwoPlayers
    | "4" -> Exit
    | _ -> Console.Clear(); printfn "\n[*] Invalid option.\n"; take ()
