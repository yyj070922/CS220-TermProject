namespace Dobutsu

open Rules

module AI = 

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
    if Board.checkWin ai board
    then 1000
    elif Board.checkWin (Piece.getOpponent ai) board
    then -1000
    else boardScore ai board + handScore ai board
  
  let chooseMoveDepth2 ai board =
    let moves = legalMoves board
    if moves = [] 
    then None 
    else
      moves
      |> List.maxBy (fun myMove ->
        let afterMyMove = applyMove board myMove
        if Board.checkWin ai afterMyMove then
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
      |> Some
