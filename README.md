# CS220-TermProject
A command-line Dobutsu Shogi built with **F# / .NET 10**.

You play as **RED** against a AI enemy **BLUE**. Enter a command(Move/Drop) to decide own behavior.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)  
  Verify with: `dotnet --version` (should show `10.x.x`)

### Run

```bash
# Windows
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

1. The following prompt is displayed, and the user can select a game mode by entering a number.
'''
Dobutsu Shogi
1. Player First
2. AI First
3. Two Players
4. Exit
'''
2. The current Dobutsu Shogi board is printed.
3. You are prompted: `Enter Behavior(Move|Drop):`
4. Type either `Move` or `Drop` and press **Enter**.
5. If you choose `Move`:
   - You are prompted: `Enter FromPos(1~12) & ToPos(1~12): _ _`
   - Enter the source position and destination position.
   - If the positions are invalid, you are asked to try again.
   - If the selected piece does not belong to you, you are asked to try again.
   - If the move is illegal for that piece, you are asked to try again.
8. If you choose `Drop`:
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

### Gamemode

Depending on the selected game mode, the gameplay proceeds as follows.
  - Player First: The player moves first, and the AI takes the enemy turn.
  - AI First: The AI moves first, and the player takes the enemy turn.
  - Two Players: Two players take turns controlling Red and Blue alternately.

### Winning & Ending

| Result | Condition |
|--------|-----------|
| **You win** | Three `O`s in a row, column, or diagonal |
| **Enemy wins** | Three `X`s in a row, column, or diagonal |
| **Tie** | All 9 squares filled with no winner |

After the game ends, you are asked whether to play again.

## LLM Part
- 기물 이동 가능 여부 코드 판단
- Move, Drop 버그 고치기
- 텍스트 색깔 입히기
- AI depth 2 구현
