namespace Dobutsu

type Move =
  | Move of fromPos: int * toPos: int
  | Drop of pos: int * piece: PieceK

module Rules = 
  let generateMoves board =
      [1..12]
      |> List.collect (fun from ->
        match board.Board.[from-1] with
        | Dropped p when p.Owner = board.Turn ->
          [1..12]
          |> List.choose (fun toPos ->
            if BoardHelper.canReach p.Owner p.Kind from toPos then
              match board.Board.[toPos-1] with
              | Empty -> Some (Move (from, toPos))
              | Dropped p2 when p2.Owner <> board.Turn -> Some (Move (from, toPos))
              | _ -> None
            else None)
        | _ -> [])
    
  let generateDrops board =
    let myHand = Map.find board.Turn board.Hand
    [1..12]
    |> List.collect (fun pos ->
      if board.Board.[pos-1] = Empty then
        myHand
        |> List.map (fun piece ->
          Drop (pos, piece))
      else [])
  
  let legalMoves board =
    generateMoves board @ generateDrops board
  
  let applyMove board move =
    let copied = Board.copy board
    let newBoard, _ = 
      match move with
      | Move (a, b) -> Board.move copied a b
      | Drop (pos, k) -> Board.drop copied pos k
    { newBoard with Turn = Piece.getOpponent newBoard.Turn }

  let lionGoalPos player =
    match player with
    | Red -> [1; 2; 3]
    | Blue -> [10; 11; 12]
  
  let findLionPos player board =
    board.Board
    |> Array.mapi (fun i slot -> i + 1, slot)
    |> Array.tryPick (fun (pos, slot) ->
      match slot with
      | Dropped { Owner = owner; Kind = Lion } when owner = player -> Some pos
      | _ -> None)
  
  let canAttackPos attacker targetPos board =
    board.Board
    |> Array.mapi (fun i slot -> i + 1, slot)
    |> Array.exists (fun (fromPos, slot) ->
      match slot with
      | Dropped { Owner = owner; Kind = kind } when owner = attacker ->
        BoardHelper.canReach owner kind fromPos targetPos
      | _ -> false)
  
  let lionReachedGoalSafely player board =
    match findLionPos player board with
    | None -> false
    | Some lionPos ->
      let enemy = Piece.getOpponent player
      List.contains lionPos (lionGoalPos player)
      && not (canAttackPos enemy lionPos board)

  let checkWin player board=
    if List.contains Lion (Map.find player board.Hand) || lionReachedGoalSafely player board
    then true 
    else false