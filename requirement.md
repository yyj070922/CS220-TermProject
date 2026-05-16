Project Title: CLI Dobutsu Shogi
Overview: This project is a command-line Dobutsu Shogi game where the user plays against an AI enemy. The user plays as Sente(first move), and the enemy plays as Gote(second move). The game is played on a 3x4 board. Each player takes turns moving one piece or placing one captured piece from their hand. The AI chooses its move by checking legal moves up to a fixed search depth.
Requirements:
1. The user will see a 3x4 Dobutsu Shogi board in the terminal
2. The board will contain the following pieces: Lion, Giraffe, Elephant, and Chick. A promoted Chick will be displayed as Hen.
3. The user will make a move by entering a command. The command will either move a piece on the board or place a captured piece from the user’s hand.
4. If the user enters an invalid command, selects an empty square, selects the enemy’s piece, moves a piece illegally, or places a captured piece illegally, the user will be asked to retry the input.
5. Each piece will move according to the rules of Dobutsu Shogi:
   - Lion can move one square in any direction.
   - Giraffe can move one square vertically or horizontally.
   - Elephant can move one square diagonally.
   - Chick can move one square forward.
   - Hen can move one square in any direction except diagonally backward
6. When a player moves a piece to a square occupied by the opponent’s piece, the opponent’s piece will be captured and added to the player’s hand.
7. When a Chick reaches the opponent’s back row, it will be promoted to Hen. A Chick does not promote when dropped.
8. When a Hen is captured, it will return to the capturer’s hand as a Chick. 
9. After the user makes a valid move, it becomes the AI enemy’s turn.
10. The AI enemy will generate all legal moves and choose a move by searching up to a fixed depth of 2 plies. At the end of the search, the AI will evaluate the board using a simple score based on piece values and winning states.
11. The user and the AI enemy will take turns until the game ends.
12. The game ends when one player captures the opponent’s Lion. If the user captures the enemy Lion, the user wins. If the AI captures the user’s Lion, the AI wins.
Example Interaction: The game prints a 3x4 board and shows the pieces in each player’s hand. The user enters a command such as “move B3 B2”. If the move is legal, the user’s piece moves to the selected square. Then the AI searches possible moves up to depth 2 and makes its move. The game prints the updated board and asks the user for the next command.
