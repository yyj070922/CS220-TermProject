# CS220-TermProject
A command-line Dobutsu Shogi built with **F# / .NET 10**.

You play as **RED** against a AI enemy, **BLUE**. Enter a command(Move/Drop) to choose your action.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)  
  Verify with: `dotnet --version` (should show `10.x.x`)

### Run

```bash
# Windows PowerShell
run.bat

# Unix / macOS
chmod +x run.sh
./run.sh

# Or directly
dotnet run
```

### Build

```bash
dotnet build
```
### Publish Self-Contained Binary

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained
```

---

## How to Play

### Board Layout

This board represents the starting position of a two-player Dobutsu Shogi.

```
+---+---+---+
| G | L | E |   1   2   3            Blue hand
+---+---+---+
|   | C |   |   4   5   6             []
+---+---+---+
|   | C |   |   7   8   9             []
+---+---+---+
| E | L | G |  10  11  12            Red hand
+---+---+---+
```

The Blue player’s pieces are placed on the top row, and the Red player’s pieces are placed on the bottom row. 
Each player has three main pieces: a Giraffe (G), a Lion (L), and an Elephant (E). 
Both players also have a Chick (C) placed near the center of the board. 
Empty spaces are shown as blank cells, and each side’s captured pieces are displayed in their respective hand areas beside the board.

### Taking a Turn

## Game Flow

1. The following prompt is displayed, and the user can select a game option by entering a number.
```
Dobutsu Shogi
1. Player First
2. AI First
3. Two Players
4. Exit
```
2. The current Dobutsu Shogi board is printed.
3. You are prompted: `Enter Behavior(Move|Drop):`
4. Type either `Move` or `Drop` and press **Enter**.
5. If you choose `Move`(or "M" | "m" | "move"):
   - You are prompted: `Enter FromPos(1~12) & ToPos(1~12): _ _`
   - Enter the source position and destination position.
   - If the positions are invalid, you are asked to try again.
   - If the selected piece does not belong to you, you are asked to try again.
   - If the move is illegal for that piece, you are asked to try again.
8. If you choose `Drop`(or | "D" | "d" | "drop"):
   - You are prompted: `Enter DropPos(1~12) & Piece(G|E|C): _ _`
   - Enter the piece from your hand and the target position.
   - If the piece is not in your hand, you are asked to try again.
   - If the target square is already occupied, you are asked to try again.
9. If the action is valid, the board is updated.
10. Captured pieces are added to the current player’s hand.
11. The turn changes to the other player.
12. The game ends when:
    - a player captures the opponent’s Lion, or
    - a player moves their Lion to the opposite end of the board without risk of being captured.

### Piece Movement

| Piece | Movement |
|--------|-----------|
| **Lion (L)** | Moves 1 square in any direction |
| **Giraffe (G)** | Moves 1 square vertically or horizontally |
| **Elephant (E)** | Moves 1 square diagonally |
| **Chick (C)** | Moves 1 square forward |
| **Hen (H)** | Moves 1 square in any direction except diagonally backward |

### Promotion

- When a **Chick (C)** reaches the opponent's end row, it is automatically promoted to a **Hen (H)**.
- A promoted **Hen (H)** returns to a normal **Chick (C)** when captured by the opponent.

### GameOption

Depending on the selected game option, the gameplay proceeds as follows.
  - Player First: The player moves first, and the AI takes the enemy turn.
  - AI First: The AI moves first, and the player moves after the AI turn
  - Two Players: Two players take turns controlling Red and Blue alternately.

### Winning & Ending

| Result | Condition |
|--------|-----------|
| **Red wins** | The opponent's Lion is captured, or Red's Lion safely reaches the enemy side |
| **Blue wins** | The opponent's Lion is captured, or Blue's Lion safely reaches the enemy side |
| **Game terminated** | A player enters `exit` during the game |

## Example Session

```
Dobutsu Shogi
1. Player First
2. AI First
3. Two Players
4. Exit
1

Turn: RED 🔴

+---+---+---+
| G | L | E |   1   2   3            Blue hand
+---+---+---+
|   | C |   |   4   5   6             []
+---+---+---+
|   | C |   |   7   8   9             []
+---+---+---+
| E | L | G |  10  11  12            Red hand
+---+---+---+

Enter Behavior(Move|Drop): Move
Enter FromPos(1~12) & ToPos(1~12): _ _ 8 5

Turn: BLUE 🔵

+---+---+---+
| G | L | E |   1   2   3            Blue hand
+---+---+---+
|   | C |   |   4   5   6             []
+---+---+---+
|   |   |   |   7   8   9             [C]
+---+---+---+
| E | L | G |  10  11  12            Red hand
+---+---+---+

AI is thinking...
...
```

## Project Structure
```
CS220-TermProject/
├── README.md
├── requirements.md
├── run.bat                 # Windows run script
├── run.sh                  # Unix run script
├── Dobutsu.fsproj          # .NET 10 F# project file
└── DobutsuShogi
    ├── Piece.fs            # Piece & PieceK & Player Type
    ├── GameOption.fs       # GameOption Type
    ├── SlotState.fs        # SlotState Type
    ├── BoardHelper.fs      # Piece move validation, Hand edit, Demote Hen
    ├── Board.fs            # BoardState Type, Copy & Drop & Move & Promote action
    ├── GameRules.fs        # Move&Drop validation, Win detection
    ├── AI.fs               # AI logic
    ├── GameInterface.fs    # Rendering, Player input validation, Gameloop
    └── Program.fs          # Entrypoint
```

### Key Types
```
// Piece type represents a game piece with its owner (`Red` or `Blue`) and kind (`Lion`, `Giraffe`, `Elephant`, `Chick`, or `Hen`).
type Piece = {
  Owner: Player
  Kind: PieceK
}
// BoardState type represents the current state of the game, including the board layout, players' hands, and the current turn.
type BoardState = {
  Board: SlotState array
  Hand: Hand
  Turn: Player
}
```

## Rules Summary

- The game is played on a **3×4 board** between **Red** and **Blue**.
- Each player controls:
  - 1 Lion (L)
  - 1 Giraffe (G)
  - 1 Elephant (E)
  - 1 Chick (C)
- Players take turns choosing either:
  - `Move` — move one of their own pieces
  - `Drop` — place a captured piece from their hand onto an empty square
- Captured pieces are added to the capturer’s hand and can later be dropped back onto the board.
- A **Chick (C)** is automatically promoted to a **Hen (H)** when it reaches the opponent’s end row.
- A captured **Hen (H)** is converted back into a normal **Chick (C)**.
- A player wins by:
  - capturing the opponent’s Lion, or
  - moving their Lion safely to the opponent’s end row without being capturable on the next turn.
- The game can be terminated at any time by entering `exit`.

## LLM Usage
- 기물 이동 가능 여부 코드 판단
- Move, Drop 버그 고치기
- 텍스트 색깔 입히기
- AI depth 2 구현
