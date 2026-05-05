namespace Dobutsu

type PieceK = Lion | Giraffe | Elephant | Chick | Hen

type Player = Red | Blue

type Piece = {
  Owner: Player
  Kind: PieceK
}

module Piece = 
  let toStrng (p: Piece) =
    let rb = 
      match p.Owner with
      | Red -> "r"
      | Blue -> "b"

    let pK = 
      match p.Kind with
      | Lion -> "L"
      | Giraffe -> "G"
      | Elephant -> "E"
      | Chick -> "C"
      | Hen -> "H"
    rb + pK
  
  let getOpponent = function
  | Red -> Blue
  | Blue -> Red



