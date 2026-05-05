namespace Dobutsu

type Hand = Map<Player, PieceK list>

type BoardState = {
  Board: SlotState array
  Hand: Hand
  Turn: Player
}

module Board = 
  let states = Array.create 12 Empty

  let Fold board folder acc = 
    board.Board
    |> Array.fold (fun (i, a) elt -> i + 1, folder i a elt) (1, acc)
    |> snd
  
  let Copy board =
    let b = Array.create 12 Empty
    let update i = function
      | Dropped m -> b.[i] <- Dropped m 
      | _ -> ()
    board.Board |> Array.iteri update
    { Board = b; Hand = board.Hand |> Map.map (fun _ lst -> List.map id lst); Turn = board.Turn }

  let PrintBoard board =
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
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |" s1 s2 s3
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |" s4 s5 s6
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |" s7 s8 s9
    printfn "+----+----+----+"
    printfn "| %s | %s | %s |" s10 s11 s12
    printfn "+----+----+----+"

  

  let Drop board num p =
    let hand = Map.find board.Turn board.Hand

    if board.Board.[num - 1] = Empty && List.contains p hand then
      let newArr = Array.copy board.Board
      let newHand = BoardHelper.removeOne p hand
      newArr.[num - 1] <- Dropped { Owner = board.Turn; Kind = p }
      { board with Board = newArr; Hand = Map.add board.Turn newHand board.Hand }, true
    else
      board, false

  let Move board n1 n2 =
    let p = board.Board.[n1 - 1]
    let a = board.Board.[n2 - 1]

    match p with
    | Empty -> board, false
    | Dropped { Owner = o1; Kind = k1 }
      when o1 = board.Turn && BoardHelper.canReach o1 k1 n1 n2 ->
      match a with
      | Empty ->
        let newArr = Array.copy board.Board
        newArr.[n1 - 1] <- Empty
        match p with
        | Dropped { Owner = owner; Kind = Chick} -> newArr.[n2 - 1] <- Dropped { Owner = owner; Kind = Hen }
        | _ -> newArr.[n2 - 1] <- p
        { board with Board = newArr }, true
      | Dropped { Owner = o2; Kind = k2 }
        when o2 <> board.Turn ->
        let newArr = Array.copy board.Board
        let captured = BoardHelper.demote k2
        let hand = Map.find o1 board.Hand
        match p with
        | Dropped { Owner = owner; Kind = Chick} -> newArr.[n2 - 1] <- Dropped { Owner = owner; Kind = Hen }
        | _ -> newArr.[n2 - 1] <- p
        { board with Board = newArr; Hand = Map.add o1 (captured :: hand) board.Hand }, true
      | _ -> board, false
    | _ -> board, false
  
      


  let Clear board num = board.Board.[num - 1] <- Empty
  
  let CheckWin player board=
    if BoardHelper.checkWinner board.Board = Some player 
    then true 
    else false