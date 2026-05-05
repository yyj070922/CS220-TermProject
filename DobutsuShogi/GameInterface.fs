namespace Dobutsu

open System
open Rules
open System.Threading

module GameInterface =
  let printBoard board =     
    let s1 = SlotState.toString board.Board.[0]
    let s2 = SlotState.toString board.Board.[1]
    let s3 = SlotState.toString board.Board.[2]
    let s4 = SlotState.toString board.Board.[3]
    let s5 = SlotState.toString board.Board.[4]
    let s6 = SlotState.toString board.Board.[5]
    let s7 = SlotState.toString board.Board.[6]
    let s8 = SlotState.toString board.Board.[7]
    let s9 = SlotState.toString board.Board.[8]
    let s10 = SlotState.toString board.Board.[9]
    let s11 = SlotState.toString board.Board.[10]
    let s12 = SlotState.toString board.Board.[11]
    let rs = 
      Map.find Red board.Hand
      |> List.map Piece.toStringK
      |> String.concat " "
    let bs = 
      Map.find Blue board.Hand
      |> List.map Piece.toStringK
      |> String.concat " "
    printfn "\n"
    match board.Turn with
      | Red -> printfn "🔴 Turn: RED"
      | Blue -> printfn "🔵 Turn: BLUE"
    printfn ""
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |         %A hand" s1 s2 s3 Blue
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |          [%s]" s4 s5 s6 bs
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |          [%s]" s7 s8 s9 rs
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |         %A hand" s10 s11 s12 Red
    printfn "+----+----+----+"
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
          printfn "\n[*] Illegal move\n"
          readPlayerMove board
      | None ->
          printfn "\n[*] Invalid input\n"
          readPlayerMove board
    
    printfn "Enter Behavior(Move|Drop):"
    Console.Write("> ")

    let input = Console.ReadLine()
    match input with
    | "exit" | "Exit" -> None
    | "Move" | "M" | "m" ->
      handleInput board 1 "Enter FromPos(1~12) & ToPos(1~12): _ _"
    | "Drop" | "D" | "d" -> 
      handleInput board 2 "Enter DropPos(1~12) & Piece(G|E|C): _ _"
    | _ -> 
      printfn "\n[*] Invalid input\n";
      readPlayerMove board
  
  let rec gameLoop board human ai =
    Console.Clear()
    printBoard board

    if Board.checkWin human board then
      printfn  "               =====================
              \n                    🎉 WIN! 🎉
              \n               ====================="
    elif Board.checkWin ai board then
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
          Thread.Sleep(500)
          printf "."
        printfn ""
        match AI.chooseMoveDepth2 ai board with
        | Some move ->
          printMove board.Turn move
          let nextBoard = applyMove board move
          gameLoop nextBoard human ai

        | None ->
          printfn "AI has no legal moves. You win!"

  let start() =
    printfn "Dobutsu Shogi"
    Thread.Sleep(500)
    printfn "Loading..."
    Thread.Sleep(1000)

    match GameOption.take () with
    | PlayerFirst ->
        let board = Board.init Red
        gameLoop board Red Blue

    | AIFirst ->
        let board = Board.init Blue
        gameLoop board Red Blue

    | Exit ->
        printfn "Bye!"
    
    