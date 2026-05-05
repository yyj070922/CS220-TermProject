namespace Dobutsu

type SlotState = 
  | Empty
  | Dropped of Piece

module SlotState = 
  let toString = function
    | Empty -> "  "
    | Dropped p -> Piece.toString p