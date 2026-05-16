namespace Dobutsu

type Hand = Map<Player, PieceK list>

type BoardState = {
  Board: SlotState array
  Hand: Hand
  Turn: Player
}

module Board = 

  let init firstTurn =
    let b = Array.create 12 Empty

    // Blue 
    b.[0] <- Dropped { Owner = Blue; Kind = Giraffe }
    b.[1] <- Dropped { Owner = Blue; Kind = Lion }
    b.[2] <- Dropped { Owner = Blue; Kind = Elephant }
    b.[4] <- Dropped { Owner = Blue; Kind = Chick }

    // Red 
    b.[7] <- Dropped { Owner = Red; Kind = Chick }
    b.[9]  <- Dropped { Owner = Red; Kind = Elephant }
    b.[10] <- Dropped { Owner = Red; Kind = Lion }
    b.[11] <- Dropped { Owner = Red; Kind = Giraffe }

    {
      Board = b
      Hand =
        Map.empty
        |> Map.add Red []
        |> Map.add Blue []
      Turn = firstTurn
    }
  
  let copy board =
    let b = Array.create 12 Empty
    let update i = function
      | Dropped m -> b.[i] <- Dropped m 
      | _ -> ()
    board.Board |> Array.iteri update
    { Board = b; Hand = board.Hand |> Map.map (fun _ lst -> List.map id lst); Turn = board.Turn }

  let drop board num p =
    let hand = Map.find board.Turn board.Hand

    if board.Board.[num - 1] = Empty && List.contains p hand then
      let newArr = Array.copy board.Board
      let newHand = BoardHelper.removeOne p hand
      newArr.[num - 1] <- Dropped { Owner = board.Turn; Kind = p }
      { board with Board = newArr; Hand = Map.add board.Turn newHand board.Hand }, true
    else
      board, false

  let promote owner kind pos =
    match owner, kind with
    | Red, Chick when pos <= 3 -> Hen
    | Blue, Chick when pos >= 10 -> Hen
    | _ -> kind

  let move board n1 n2 =
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
        newArr.[n2 - 1] <- Dropped { Owner = o1; Kind = promote o1 k1 n2}
        { board with Board = newArr }, true
      | Dropped { Owner = o2; Kind = k2 }
        when o2 <> board.Turn ->
        let newArr = Array.copy board.Board
        let captured = BoardHelper.demote k2
        let hand = Map.find o1 board.Hand
        newArr.[n1 - 1] <- Empty
        newArr.[n2 - 1] <- Dropped { Owner = o1; Kind = promote o1 k1 n2}
        { board with Board = newArr; Hand = Map.add o1 (captured :: hand) board.Hand }, true
      | _ -> board, false
    | _ -> board, false

  