module Dobutsu.BoardHelper

let checkWinner states =
  let rLion = Dropped { Owner = Red; Kind = Lion }
  let bLion = Dropped { Owner = Blue; Kind = Lion }
  if Array.contains rLion states = false
  then Some Red
  elif Array.contains bLion states = false
  then Some Blue
  else None

let toCoord n =
  let i = n - 1
  i / 3, i % 3   // row, col

let toNumber (row, col) =
  row * 3 + col + 1

let inBounds (row, col) =
  row >= 0 && row < 4 && col >= 0 && col < 3

let directions player kind =
  let forward =
    match player with
    | Red -> -1
    | Blue -> 1

  match kind with
  | Chick -> [ forward, 0 ]
  | Hen ->
    [ forward,-1; forward,0; forward,1;
      0,-1;                  0,1;
                  -forward,0            ]
  | Lion ->
    [ -1,-1; -1,0; -1,1
      0,-1;        0,1
      1,-1;  1,0;  1,1 ]
  | Giraffe ->
    [ -1,0; 0,-1; 0,1; 1,0 ]
  | Elephant ->
    [ -1,-1; -1,1; 1,-1; 1,1 ]

let canReach player kind n1 n2 =
  let row, col = toCoord n1

  directions player kind
  |> List.map (fun (dr, dc) -> row + dr, col + dc)
  |> List.filter inBounds
  |> List.map (fun (r, c) -> r * 3 + c + 1)
  |> List.contains n2

let removeOne p hand =
  let rec loop acc = function
    | [] -> List.rev acc
    | x :: xs when x = p -> List.rev acc @ xs
    | x :: xs -> loop (x :: acc) xs
  loop [] hand

let demote kind =
  match kind with
  | Hen -> Chick
  | _ -> kind
