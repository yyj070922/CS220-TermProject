namespace Dobutsu

open System

type GameOption = PlayerFirst | AIFirst | Exit

module GameOption =
  let rec take () =
    printfn "1. Player First"
    printfn "2. AI First"
    printfn "3. Exit"
    Console.Write "> "
    match Console.ReadLine () with
    | "1" -> PlayerFirst
    | "2" -> AIFirst
    | "3" -> Exit
    | _ -> printfn "\n[*] Invalid option.\n"; take ()
