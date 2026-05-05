namespace Dobutsu

type Move =
  | Move of fromPos: int * toPos: int
  | Drop of pos: int * piece: PieceK

module AI = 
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
    let copied = Board.Copy board
    let newBoard, _ = 
      match move with
      | Move (a, b) -> Board.Move copied a b
      | Drop (pos, k) -> Board.Drop copied pos k
    newBoard

  let pieceScore = function
    | Lion -> 100
    | Giraffe -> 5
    | Elephant -> 5
    | Hen -> 3
    | Chick -> 1
  
  let boardScore ai board =
    board.Board
    |> Array.sumBy (function
      | Empty -> 0
      | Dropped p ->
        let s = pieceScore p.Kind
        if p.Owner = ai then s else -s)

  let handScore ai board =
    board.Hand
    |> Map.toList
    |> List.sumBy (fun (player, pieces) ->
      pieces
      |> List.sumBy (fun kind ->
        let s = pieceScore kind / 2
        if player = ai then s else -s))

  let evaluate ai board =
    if Board.CheckWin ai board
    then 1000
    elif Board.CheckWin (Piece.getOpponent ai) board
    then -1000
    else boardScore ai board + handScore ai board
  
  let chooseMoveDepth2 ai board =
    let moves = legalMoves board
    moves
    |> List.maxBy (fun myMove ->
      let afterMyMove = applyMove board myMove
      if Board.CheckWin ai afterMyMove then
        1000
      else
        let enemyMoves = legalMoves afterMyMove
        if List.isEmpty enemyMoves then
          evaluate ai afterMyMove
        else
          enemyMoves
          |> List.map (fun enemyMove ->
            let afterEnemyMove = applyMove afterMyMove enemyMove
            evaluate ai afterEnemyMove)
          |> List.min)
