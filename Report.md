Contents:
- ![Inroduction](https://github.com/deniz-kaya/battleboats/blob/master/Report.md#introduction)
- ![Documented Design](https://github.com/deniz-kaya/battleboats/blob/master/Report.md#documented-design)
- Technical Solution
- Testing
- ![Evaluation](https://github.com/deniz-kaya/battleboats/blob/master/Report.md#evaluation)

- # Introduction
	- ## Scenario:
	- Creating a digital version of the board game battleboats (NOT ships) using a C# console app, where the player plays against a computer.
	- ## Project requirements:
		- **A user-friendly menu of options to navigate through the program.**
			- Clear language
			- Instructions
			- Ability to quit the program
		- **A fully functioning battle boats game, which consists of:**
			- Ability to attack
			- Preventing players from attacking cells that have already been attacked
			- Ability to display the tracker board and own board for the players
			- Showing players the attacks of the opponent and their attacks and their result.
			- Ability to place ships
			- Preventing users from placing down ships where a ship already exists
			- Preventing users from placing ships outside the bounds of the board
			- Ability to detect winners
			- Detecting how many cells have been hit
			- Statistics and trackers
			- Variables such as the turn number, how many times they have been hit, missed etc.
		- **Ability of user interaction**
			- Input validation e.g. inputting coordinates
		- **Saving and reloading of the game**
			- Saving per turn so that progress can be continued if the user decides to stop playing mid-game
			- Seamless loading of the game with just the filepath of a save file.
- # Documented Design
	- ![project flow diagram.svg](https://github.com/deniz-kaya/battleboats/blob/master/project%20flow%20diagram.svg)
	- Note: variable and subroutine parameter and return type were generated by AI, however the definitions and function explanations were written by hand.
	- Game.cs
		- **Class: Computer** (derived from Player)
			- Variable: `notChosen`
			- Subroutine: `ResetNotChosen()`
				- Parameters: None
				- Return Type: Void
				- Function: Resets the list of unchosen spots on the board.
			- Subroutine: `Computer()`
				- Parameters: None
				- Return Type: Void
				- Function: Initializes the Computer object.
			- Subroutine: `ChooseSpot()`
				- Parameters: None
				- Return Type: Tuple (int x, int y)
				- Function: Chooses a position from a random index of notChosen, removes the chosen value so that it cannot be chosen again.
			- Subroutine: `PlaceShips()`
				- Parameters: None
				- Return Type: Void
				- Function: Places ships randomly, with valid random positions and orientations.
			- Subroutine: `Display()`
				- Parameters: None
				- Return Type: Void
				- Function: Does nothing, definition needed for functionality as it is an abstract subroutine within player.
		- **Class: Human** (derived from Player)
			- Subroutine: `GetOrientation(string ans)`
				- Parameters: string ans
				- Return Type: Ship.Orientation
				- Function: Returns the orientation represented by the string in the parameter.
			- Subroutine: `CheckOrientation(string ans)`
				- Parameters: string ans
				- Return Type: Boolean
				- Function: Returns true if the string in the parameter represents a valid orientation of a ship.
			- Subroutine: `GetPosition(string ans)`
				- Parameters: string ans
				- Return Type: Tuple (int x, int y)
				- Function: Returns the position coordinate represented by the string in the parameter.
			- Subroutine: `CheckPosition(string ans)`
				- Parameters: string ans
				- Return Type: Boolean
				- Function: Returns true if the string in the parameter represents a valid position coordinate on the board.
			- Subroutine: `ChooseSpot()`
				- Parameters: None
				- Return Type: Tuple (int x, int y)
				- Function: Returns a position where the tracker board is empty, chosen by the player.
			- Subroutine: `PlaceShips()`
				- Parameters: None
				- Return Type: Void
				- Function: Prompts user to place all ships given in FleetTemplate until all ships are placed correctly.
			- Subroutine: `Display()`
				- Parameters: None
				- Return Type: Void
				- Function: Sketches the board.
		- **Class: Player**
			- Variable: `FleetTemplate`
				- Type: List<Ship>
				- Description: Template of the fleet. This will be used for the PlaceShips() subroutine.
			- Variable: `FleetTotalSize`
				- Type: int
				- Description: Total size of the template fleet.
			- Variable: `LetterToNumber`
				- Type: Dictionary<char, int>
				- Description: Mapping of letters to numbers.
			- Variable: `board`
				- Type: Cell[,]
				- Description: Board for the player.
			- Variable: `trackerBoard`
				- Type: Cell[,]
				- Description: Tracker board for the player.
			- Variable: `TrackerHit`
				- Type: int
				- Description: Number of hits on the tracker board.
			- Variable: `TrackerMiss`
				- Type: int
				- Description: Number of misses on the tracker board.
			- Variable: `SelfHit`
				- Type: int
				- Description: Number of hits on the player's own board.
			- Variable: `SelfMiss`
				- Type: int
				- Description: Number of misses on the player's own board.
			- Subroutine: `SetPlayerData(PlayerData data)`
				- Parameters: PlayerData data
				- Return Type: Void
				- Function: Sets the player's data.
			- Subroutine: `GetPlayerData()`
				- Parameters: None
				- Return Type: PlayerData
				- Function: Retrieves the player's data.
			- Subroutine: `Player()`
				- Parameters: None
				- Return Type: Void
				- Function: Initializes the Player object.
			- Subroutine: `SketchBoards()`
				- Parameters: None
				- Return Type: Void
				- Function: Sketches the board state for the player.
			- Subroutine: `TryPlace(Ship ship)`
				- Parameters: Ship ship
				- Return Type: Boolean
				- Function: Tries to place the ship given in the parameter on the board. Returns true if placement was successful.
			- Subroutine: `IsValidShipPosition(Ship ship)`
				- Parameters: Ship ship
				- Return Type: Boolean
				- Function: Returns true if the ship is within the bounds of the board and that another ship doesn't occupy it on the board.
			- Subroutine: `CheckForProperty(Ship ship, Cell property)`
				- Parameters: Ship ship, Cell property
				- Return Type: Boolean
				- Function: Returns true if all spots where the ship would be are of property given in the parameter.
			- Subroutine: `BakePlacement(Ship ship)`
				- Parameters: Ship ship
				- Return Type: Void
				- Function: Bakes the ship position onto the self board.
			- Subroutine: `IncomingAttack((int x, int y) pos)`
				- Parameters: Tuple (int x, int y) pos
				- Return Type: Boolean
				- Function: Returns true if a ship exists in the own board at the position in the parameter. Changes the property of the Cell at the position, to either being a hit or a miss.
			- Subroutine: `Attack(Player opponent)`
				- Parameters: Player opponent
				- Return Type: Boolean
				- Function: Sends an attack to the player given in the parameter. Given the attack success, it changes the tracker board and the TrackerHit/Miss variables accordingly.
			- Subroutine: `IsWinner()`
				- Parameters: None
				- Return Type: Boolean
				- Function: Checks if the current player's TrackerHit variable matches up with the FleetTotalSize variable. If it does, true is returned and player is considered
			- Subroutine: `CheckAttackPosition((int x, int y) pos)`
				- Parameters: Tuple (int x, int y) pos
				- Return Type: Boolean
				- Function: Returns true if the tracker board is empty at the given position.
			- Abstract Subroutine: `ChooseSpot()`
				- Parameters: None
				- Return Type: Tuple (int x, int y)
				- Function: Abstract: Returns a position where the tracker board is empty, chosen by the player.
			- Abstract Subroutine: `PlaceShips()`
				- Parameters: None
				- Return Type: Void
				- Function: Abstract: Prompts player to place all ships given in FleetTemplate until all ships are placed correctly.
			- Abstract Subroutine: `Display()`
				- Parameters: None
				- Return Type: Void
				- Function: Abstract: Displays relevant information about the state of the game visible to that player.
		- **Class: Game**
			- Variable: `player1`
				- Type: Human
				- Description: First, human player.
			- Variable: `player2`
				- Type: Computer
				- Description: Second, computer player.
			- Variable: `turnCount`
				- Type: int
				- Description: Count of turns.
			- Variable: `savepath`
				- Type: string
				- Description: File path where the game is saved.
			- Subroutine: `SaveGame(string filepath)`
				- Parameters: string filepath
				- Return Type: Boolean
				- Function: Creates a GameState instance of the current GameState, and tries to serialize and write it to the file at the filepath given in the parameter. Returns true if save is successful.
			- Subroutine: `LoadGame(string filepath)`
				- Parameters: string filepath
				- Return Type: Boolean
				- Function: Tries to deserialize and load the game state given in the file in the filepath. If the deserialization fails, error message is shown to user and user is returned to the main menu. If the file at filepath doesn't exist, a game save is created at that file, and the game is initiated.
			- Subroutine: `Game()`
				- Parameters: None
				- Return Type: Void
				- Function: Initializes the Game object.
			- Subroutine: `Game(string filepath)`
				- Parameters: string filepath
				- Return Type: Void
				- Function: Initializes the Game object with a specific filepath to load a saved game.
			- Subroutine: `InitialiseGame()`
				- Parameters: None
				- Return Type: Void
				- Function: Initializes the game by placing the ships for each player.
			- Subroutine: `GameOver(bool player1Won)`
				- Parameters: Boolean player1Won
				- Return Type: Void
				- Function: Displays the correct winner message given the parameter.
			- Subroutine: `Play()`
				- Parameters: None
				- Return Type: Void
				- Function: Plays the game until either player is the winner. The game is saved between each turn, so that if the player exits the program, they can continue their save from where they left off. When either player wins the game, the GameOver subroutine is called and afterwards the user returns to the main menu.
		- **Enum: Cell**
			- Empty: Represents empty spot on the board
			- Ship: Represents a ship spot on the board
			- Miss: Represents a spot that was a miss on the board
			- Hit: represents a spot that has been hit on the board
		- **Struct: Ship**
			- Variable: `size`
				- Type: int
				- Description: Size of the ship.
			- Variable: `orientation`
				- Type: Ship.Orientation
				- Description: Orientation of the ship.
			- Variable: `position`
				- Type: Tuple (int x, int y)
				- Description: Position of the ship on the board.
		- **Struct: GameState**
			- Variable: `player1Data`
				- Type: PlayerData
				- Description: Data for the first player.
			- Variable: `player2Data`
				- Type: PlayerData
				- Description: Data for the second player.
			- Variable: `turnCount`
				- Type: int
				- Description: Count of turns.
			- Variable: `notChosen`
				- Type: List<int>
				- Description: List of not chosen spots on the board for the computer player.
		- **Struct: PlayerData**
			- Variable: `board`
				- Type: Cell[,]
				- Description: Board for the player.
			- Variable: `trackerBoard`
				- Type: Cell[,]
				- Description: Tracker board for the player.
			- Variable: `TrackerHit`
				- Type: int
				- Description: Number of hits on the tracker board.
			- Variable: `TrackerMiss`
				- Type: int
				- Description: Number of misses on the tracker board.
			- Variable: `SelfHit`
				- Type: int
				- Description: Number of hits on the player's own board.
				- Variable: `SelfMiss`
					- Type: int
					- Description: Number of misses on the player's own board.
	- Renderer.cs
		- **Class: Renderer**
			- Enum: `FG`
				- Type: int
				- Description: Colour name to ANSI 4-bit foreground colour integer.
			- Enum: `BG`
				- Type: int
				- Description: Colour name to ANSI 4-bit backgound colour integer.
			- Variable: `DefaultForeground`
				- Type: `FG`
				- Description: The default foreground color.
			- Variable: `DefaultBackground`
				- Type: `BG`
				- Description: The default background color.
			- Variable: `sketchPad`
				- Type: `string`
				- Description: The string that stores the sketched text.
			- Subroutine: `SetDefaultColor()`
				- Parameters: `FG DefaultForeground, BG DefaultBackground`
				- Return Type: Void
				- Function: Sets the default color values for the text.
			- Subroutine: `Draw()`
				- Parameters: `(int x, int y) pos`
				- Return Type: Void
				- Function: Prints the contents of the sketchPad string on the requested position.
			- Subroutine: `Sketch()`
				- Parameters: `string text, FG ForegroundColor, BG BackgroundColor`
				- Return Type: Void
				- Function: Adds to sketchPad without line break.
			- Subroutine: `SketchLine()`
				- Parameters: `string text, FG ForegroundColor, BG BackgroundColor`
				- Return Type: Void
				- Function: Adds to sketchPad with line break.
			- Subroutine: `WriteLine()`
				- Parameters: `string text, FG ForegroundColor, BG BackgroundColor`
				- Return Type: Void
				- Function: Writes to console with line break.
			- Subroutine: `Write()`
				- Parameters: `string text, FG ForegroundColor, BG BackgroundColor`
				- Return Type: Void
				- Function: Writes to console without line break.
	- UI.cs
		- **Class: UI**
			- Enum: `Request`
				- Type: int
				- Variables:
					- `NewQuickGame = 1`
					- `ResumeQuickGame = 2`
					- `LoadGame = 3`
					- `Instructions = 4`
					- `Quit = 5`
			- Variable: `printer`
				- Type: `Renderer.Printer`
				- Description: Printer object for rendering text.
			- Subroutine: `UI()`
				- Parameters: `Renderer.FG foreground, Renderer.BG background`
				- Return Type: Void
				- Function: Constructor for the UI class, sets default colour values for the Renderer.
			- Subroutine: `Instructions()`
				- Parameters: None
				- Return Type: Void
				- Function: Prints a whole chunk of text, instructions.
			- Subroutine: `GetFilepath()`
				- Parameters: None
				- Return Type: string
				- Function: Gets the file path of the game save.
			- Subroutine: `Menu()`
				- Parameters: None
				- Return Type: Request
				- Function: Displays menu and returns request made by the user.
- # Technical Solution
	- Please see files attached with this document, or check the github: https://github.com/deniz-kaya/battleboats
- # Testing
	- Please see the attached video.
- # Evaluation
	- Overall, I think that the project has been successful. It achieves all of the project requirements as it has a user friendly menu of options to navigate the program, a fully functional battle boats game with all core features, user interactions with input validation and successful saving and loading. The technical solution is well-documented and human-legible (this means no voodoo one liners and incomprehensible code). However, I believe that the project has had a few shortcomings, as listed below.
	- If I were to do the project again, I would primarily take to mind the processing time that saving the game takes. This task could be done asynchronously to provide a more seamless experience for the user. Similarly, clearing and re-printing the boards causes a noticeable delay, a system could be set in place where only the values that have been changed are printed at their respective positions on the console window, and therefore the experience is more seamless.
	- At the end of the day, this project has improved my skills within object oriented programming as I had to research certain aspects of C# data types, prominently classes. Although there were points at which my code could be improved for a better user interface, what I was able to produce is still very workable and functional, so I would consider it a success.
	- Shortcomings:
		- Noticeable delay between certain parts of the program e.g. when rendering the updated boards, when saving the game.
		- Very basic features e.g. doesn't show when a ship is sunk.
	- Potential improvements:
		- Customisable fleet.
		- Visual personalisation, e.g. change colours of sunken ships, default text colour etc.
		- Human vs Human game mode instead of just Human vs Computer.
		- Game saves menu where user can easily create and load saves (save slots).
		- Arrow-key user interactions instead of coordinate and number etc.
		- Asynchronous game saves.
		- Showing when a ship is sunk.
		- Updating changes on boards individually instead of clearing and printing the whole thing again.
