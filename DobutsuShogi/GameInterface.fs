namespace Dobutsu

open System
open Rules
open System.Threading

module GameInterface =
  let printSlot slot =
    match slot with
    | Empty ->
      printf "   "
    | Dropped {Owner = Red; Kind = kind} ->
      Console.ForegroundColor <- ConsoleColor.Red
      printf " %s " (Piece.toStringK kind)
      Console.ResetColor()
    | Dropped {Owner = Blue; Kind = kind} ->
      Console.ForegroundColor <- ConsoleColor.Blue
      printf " %s " (Piece.toStringK kind)
      Console.ResetColor()
  let printBoard board =     
    let rs = 
      Map.find Red board.Hand
      |> List.map Piece.toStringK
      |> String.concat " "
    let bs = 
      Map.find Blue board.Hand
      |> List.map Piece.toStringK
      |> String.concat " "
    printfn ""
    printf "Turn: "
    match board.Turn with
      | Red -> 
        Console.ForegroundColor <- ConsoleColor.Red
        printfn "RED 🔴"
      | Blue -> 
        Console.ForegroundColor <- ConsoleColor.Blue
        printfn "BLUE 🔵"
    Console.ResetColor()
    printfn ""
    printfn "+---+---+---+"
    printf "|"; printSlot board.Board.[0]; printf "|"; printSlot board.Board.[1]; printf "|"; printSlot board.Board.[2]; printfn "|   1   2   3            %A hand" Blue
    printfn "+---+---+---+"
    printf "|"; printSlot board.Board.[3]; printf "|"; printSlot board.Board.[4]; printf "|"; printSlot board.Board.[5]; printfn "|   4   5   6             [%s]" bs
    printfn "+---+---+---+"
    printf "|"; printSlot board.Board.[6]; printf "|"; printSlot board.Board.[7]; printf "|"; printSlot board.Board.[8]; printfn "|   7   8   9             [%s]" rs
    printfn "+---+---+---+"
    printf "|"; printSlot board.Board.[9]; printf "|"; printSlot board.Board.[10]; printf "|"; printSlot board.Board.[11]; printfn "|  10  11  12            %A hand" Red
    printfn "+---+---+---+"
    printfn ""
    printfn "---------------------------------------------------"
  
  let printMove turn move =
    match move with
    | Move (fromPos, toPos) ->
        printfn "%A moves: %d -> %d" turn fromPos toPos

    | Drop (pos, kind) ->
        printfn "%A drops %s at %d" turn (Piece.toStringK kind) pos

  let tryParseMove cmd (input: string) =
    let parts =
      input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries)
      |> Array.toList

    match cmd, parts with
    | 1, [fromPos; toPos] ->
        match Int32.TryParse fromPos, Int32.TryParse toPos with
        | (true, f), (true, t) -> Some (Move (f, t))
        | _ -> None

    | 2, [pos; kind] ->
      match Int32.TryParse pos with
      | true, p ->
        match kind.ToUpper() with
        | "G" -> Some (Drop (p, Giraffe))
        | "E" -> Some (Drop (p, Elephant))
        | "C" -> Some (Drop (p, Chick))
        | _ -> None
      | _ -> None
    | _ -> None

  

  let rec readPlayerMove board =
    let rec handleInput board cmd prompt =
      printfn "%s" prompt
      Console.Write("> ")

      let input = Console.ReadLine()
      let parsed = tryParseMove cmd input

      match parsed with
      | Some m when List.contains m (legalMoves board) ->
          Some m
      | Some _ ->
        Console.Clear()
        printBoard board
        printfn "\n[*] Illegal move\n"
        readPlayerMove board
      | None ->
        Console.Clear()
        printBoard board
        printfn "\n[*] Invalid input\n"
        readPlayerMove board

    printfn "Enter Behavior(Move|Drop):"
    Console.Write("> ")

    let input = Console.ReadLine()
    match input with
    | "exit" | "Exit" -> None
    | "Move" | "M" | "m" | "move" ->
      Console.Clear()
      printBoard board
      handleInput board 1 "Enter FromPos(1~12) & ToPos(1~12): _ _"
    | "Drop" | "D" | "d" | "drop" -> 
      Console.Clear()
      printBoard board
      handleInput board 2 "Enter DropPos(1~12) & Piece(G|E|C): _ _"
    | _ -> 
      Console.Clear()
      printBoard board
      printfn "\n[*] Invalid input\n";
      readPlayerMove board
  
  let rec gameLoop board human ai =
    Console.Clear()
    printBoard board

    if checkWin human board then
      printfn  "               =====================
              \n                    🎉 WIN! 🎉
              \n               ====================="
    elif checkWin ai board then
      printfn  "               =====================
              \n                   😭 LOSE.. 😭
              \n               ====================="
    else
      if board.Turn = human then
        let move = readPlayerMove board
        match move with
        | None -> 
          printfn "Game terminated."
        | Some move ->
          printMove board.Turn move
          let nextBoard = applyMove board move
          gameLoop nextBoard human ai

      else
        printf "AI is thinking"
        for _ in 1..6 do
          Thread.Sleep 500
          printf "."
        printfn ""
        match AI.chooseMoveDepth2 ai board with
        | Some move ->
          printMove board.Turn move
          let nextBoard = applyMove board move
          gameLoop nextBoard human ai

        | None ->
          printfn "AI has no legal moves. You win!"
  let rec gameLoopTP board turn next =
    Console.Clear()
    printBoard board

    if checkWin Red board then
      printfn  "               =======================
              \n                   🎉 Red WIN! 🎉
              \n               =======================" 
    elif checkWin Blue board then
      printfn  "               ========================
              \n                   🎉 Blue WIN! 🎉
              \n               ========================" 
    else
      let move = readPlayerMove board
      match move with
      | None -> 
        printfn "Game terminated."
      | Some move ->
        printMove board.Turn move
        let nextBoard = applyMove board move
        gameLoopTP nextBoard next turn

  let start() =
    Console.Clear()
    printfn "Dobutsu Shogi"
    Thread.Sleep 500
    printfn "Loading..."
    Thread.Sleep 1000
    Console.Clear()

    match GameOption.take () with
    | PlayerFirst ->
      let board = Board.init Red
      gameLoop board Red Blue
    | AIFirst ->
      let board = Board.init Blue
      gameLoop board Red Blue
    | TwoPlayers ->
      let board = Board.init Red
      gameLoopTP board Red Blue
    | Exit ->
      printfn "Bye!"
    
    