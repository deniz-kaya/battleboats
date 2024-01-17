using Newtonsoft.Json;

namespace BattleBoats
{
    public class Computer : Player
    {
        //List of not chosen spots on the board.
        public List<int> notChosen = new();

        //Self explanatory.
        private void ResetNotChosen()
        {
            notChosen = new();
            for (int i = 0; i < 64; i++)
            {
                notChosen.Add(i);
            }
        }
        public Computer()
        {
            ResetNotChosen();
        }
        //Chooses a position from a random index of notChosen, removes the chosen value so that it cannot be chosen again.
        public override (int x, int y) ChooseSpot()
        {
            Random rnd = new();
            int randomIndex = rnd.Next(0, notChosen.Count);
            int temp = notChosen[randomIndex];
            notChosen.RemoveAt(randomIndex);
            return (temp % 8, temp / 8);
        }

        //Places ships randomly, with valid random positions and orientations. 
        public override void PlaceShips()
        {
            Random rnd = new();
            for (int i = 0; i < FleetTemplate.Count; i++)
            {
                Ship temp = FleetTemplate[i];
                bool sucessfulPlacement = false;
                while (!sucessfulPlacement)
                {
                    temp.position = (rnd.Next(0, 8), rnd.Next(0, 8));
                    temp.orientation = (rnd.Next(1, 3) == 1) ? Ship.Orientation.Horizontal : Ship.Orientation.Vertical;
                    sucessfulPlacement = TryPlace(temp);
                }
            }
        }
        //Left empty, as the computer does not need to display anything for its turn.
        public override void Display()
        {
            //computer does not need to display anything, therefore it is empty.
            return;
        }
    }
    public class Human : Player
    {
        //Returns the orientation represented by the strin gin the parameter.
        private Ship.Orientation GetOrientation(string ans)
        {
            return ans == "H" ? Ship.Orientation.Horizontal : Ship.Orientation.Vertical;
        }

        //Returns true if the string in parameter represents a valid orientation of a ship.
        private bool CheckOrientation(string ans)
        {
            if (ans.Length != 1)
            {
                return false;
            }
            if (ans == "H" | ans == "V")
            {
                return true;
            }
            return false;
        }

        //Returns the position coordinate represented by the string in parameter.
        private (int x, int y) GetPosition(string ans)
        {
            int x = Player.LetterToNumber[ans[0]] - 1;
            int y = (int)(ans[1] - '1');
            return (x, y);
        }

        //Returns true if the string in parameter represents a valid position coordinate on the board.
        private bool CheckPosition(string ans)
        {
            if (ans.Length != 2)
            {
                return false;
            }
            if (!(Char.IsLetter(ans[0]) & Char.IsDigit(ans[1])))
            {
                return false;
            }

            int x = (int)(ans[0] - 'A');
            int y = (int)(ans[1] - '1');

            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
            {
                return true;
            }
            return false;
        }
        
        //Returns a position where the tracker board is empty, chosen by the player.
        public override (int x, int y) ChooseSpot()
        {
            bool validPosition = true;
            bool legalPosition;
            (int x, int y) pos;
            string positionAns;
            //Until the position is legal,
            do
            {
                Console.SetCursorPosition(0, 14);
                //Until a valid position is entered,
                do
                {
                    //Prompt user to enter a position.
                    if (!validPosition)
                    {
                        printer.WriteLine("\nInvalid position coordinate!", Renderer.FG.BrightRed);
                    }
                    printer.Write($"Input attack position: ");
                    positionAns = Console.ReadLine().ToUpper();

                    validPosition = CheckPosition(positionAns);
                } while (!validPosition);

                pos = GetPosition(positionAns);
                legalPosition = CheckAttackPosition(pos);

            } while (!legalPosition);

            return pos;
        }
        
        //Prompts user to place all ships given in FleetTemplate until all ships are placed correctly.
        public override void PlaceShips()
        {
            //For each ship in the FleetTemplate:
            for (int i = 0; i < FleetTemplate.Count; i++)
            {
                Console.Clear();
                SketchBoards();
                bool successfulPlacement;
                Console.SetCursorPosition(0, 14);
                //Until there is a successful placement,
                do
                {
                    bool validAnswer = true;
                    string positionAns;
                    string orientationAns;

                    //Until there is a valid position and orientation,
                    do
                    {
                        //Prompt the player to enter a position and orientation.
                        if (!validAnswer)
                        {
                            printer.WriteLine("Bad position and/or orientation!", Renderer.FG.BrightRed);
                        }
                        printer.Write($"Input position of ship (Size: {FleetTemplate[i].size}): ");
                        positionAns = Console.ReadLine().ToUpper();
                        printer.Write($"Input orientation of ship (H/V): ");
                        orientationAns = Console.ReadLine().ToUpper();
                        printer.WriteLine("");
                        validAnswer = CheckPosition(positionAns) & CheckOrientation(orientationAns);
                    }
                    while (!validAnswer);
                    
                    //Creates a test ship with user position and orientation using the template ship and tries to place it.
                    Ship test = FleetTemplate[i];
                    test.position = GetPosition(positionAns);
                    test.orientation = GetOrientation(orientationAns);
                    successfulPlacement = TryPlace(test);
                    if (!successfulPlacement)
                    {
                        printer.WriteLine("Illegal position!", Renderer.FG.BrightRed);
                    }
                }
                while (!successfulPlacement);
            }

        }

        //Sketches the board. 
        public override void Display()
        {
            SketchBoards();
        }
    }
    public abstract class Player
    {
        //Used to represent a ship within a fleet.
        public struct Ship
        {
            public enum Orientation
            {
                Horizontal = 0,
                Vertical = 1,
            }
            public int size;
            public Orientation orientation;
            public (int x, int y) position;
        }

        //Self explanatory.
        public void SetPlayerData(PlayerData data)
        {
            this.board = data.board;
            this.trackerBoard = data.trackerBoard;
            this.SelfHit = data.SelfHit;
            this.SelfMiss = data.SelfMiss;
            this.TrackerHit = data.TrackerHit;
            this.TrackerMiss = data.TrackerMiss;
        }
        
        //Self explanatory.
        public PlayerData GetPlayerData()
        {
            PlayerData data;
            data.board = this.board;
            data.trackerBoard = this.trackerBoard;
            data.SelfMiss = this.SelfMiss;
            data.TrackerMiss = this.TrackerMiss;
            data.SelfHit = this.SelfHit;
            data.TrackerHit = this.TrackerHit;
            return data;
        }

        //Static readonly variables that can be tweaked easily within code.
        //Existence also creates opportunity for a future option where players can create their own sized boards and ships within a menu.

        public readonly static List<Ship> FleetTemplate = new()
        {
            new Ship
            {
                size = 1,                
            },
            new Ship
            {
                size = 1,                
            },
            new Ship
            {
                size = 2,                
            },
            new Ship
            {
                size = 2,                
            },
            new Ship
            {
                size = 3,                
            }
        };
        public readonly static int FleetTotalSize = 9;
        public readonly static Dictionary<char, int> LetterToNumber = new()
        {
            { 'A', 1 },
            { 'B', 2 },
            { 'C', 3 },
            { 'D', 4 },
            { 'E', 5 },
            { 'F', 6 },
            { 'G', 7 },
            { 'H', 8 },
        };

        //Constructor initialises both boards to be empty. 
        public Player()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = Cell.Empty;
                    trackerBoard[i, j] = Cell.Empty;
                }
            }
        }

        //Tracker variables,
        public int TrackerHit, TrackerMiss;
        public int SelfHit, SelfMiss;

        //Boards
        private Cell[,] board = new Cell[8, 8];
        private Cell[,] trackerBoard = new Cell[8, 8];

        //SKetches the board state for the player.
        public void SketchBoards()
        {
            //Sketches and draws the own board on the console.
            printer.SketchLine("Fleet:");
            printer.Sketch(" A B C D E F G H ");
            for (int y = 0; y < 8; y++)
            {
                printer.Sketch($"\n{y + 1}");
                for (int x = 0; x < 8; x++)
                {
                    switch (board[x, y])
                    {
                        case Cell.Empty:
                            printer.Sketch($"~", Renderer.FG.Blue, Renderer.BG.BrightBlue);
                            printer.Sketch($"~", Renderer.FG.Blue, Renderer.BG.BrightBlue);
                            break;
                        case Cell.Miss:
                            printer.Sketch($"(", Renderer.FG.Black, Renderer.BG.BrightBlue);
                            printer.Sketch($")", Renderer.FG.Black, Renderer.BG.BrightBlue);
                            break;
                        case Cell.Hit:
                            printer.Sketch($"[", Renderer.FG.Red, Renderer.BG.Blue);
                            printer.Sketch($"]", Renderer.FG.Red, Renderer.BG.Blue);
                            break;
                        case Cell.Ship:
                            printer.Sketch($"[", Renderer.FG.White, Renderer.BG.Blue);
                            printer.Sketch($"]", Renderer.FG.White, Renderer.BG.Blue);
                            break;
                    }
                }
            }
            printer.Sketch($"\nHit: {SelfHit}\nMiss: {SelfMiss}");
            printer.Draw((2, 1));

            //Sketches and draws the tracker board on the console.
            printer.SketchLine("Tracker:");
            printer.Sketch(" A B C D E F G H ");
            for (int y = 0; y < 8; y++)
            {
                printer.Sketch($"\n{y + 1}");
                for (int x = 0; x < 8; x++)
                {
                    switch (trackerBoard[x, y])
                    {
                        case Cell.Empty:
                            printer.Sketch($"~", Renderer.FG.Blue, Renderer.BG.BrightBlue);
                            printer.Sketch($"~", Renderer.FG.Blue, Renderer.BG.BrightBlue);
                            break;
                        case Cell.Miss:
                            printer.Sketch($"(", Renderer.FG.Black, Renderer.BG.BrightBlue);
                            printer.Sketch($")", Renderer.FG.Black, Renderer.BG.BrightBlue);
                            break;
                        case Cell.Hit:
                            printer.Sketch($">", Renderer.FG.Red, Renderer.BG.Blue);
                            printer.Sketch($"<", Renderer.FG.Red, Renderer.BG.Blue);
                            break;
                        case Cell.Ship:
                            printer.Sketch($"[", Renderer.FG.Red, Renderer.BG.Blue);
                            printer.Sketch($"]", Renderer.FG.Red, Renderer.BG.Blue);
                            break;
                    }
                }
            }
            printer.Sketch($"\nHit: {TrackerHit}\nMiss: {TrackerMiss}");
            printer.Draw((25, 1));
        }

        //Tries to place the ship given in the parameter on the board. Returns true if placement was successful. 
        public bool TryPlace(Ship ship)
        {
            if (IsValidShipPosition(ship))
            {
                BakePlacement(ship);
                return true;
            }
            else
            {
                return false;
            }
        }

        //IsValidShipPosition returns true if the ship is within the bounds of the board and that another ship doesnt occupy it on the board.
        private bool IsValidShipPosition(Ship ship)
        {
            (int x, int y) pos = ship.position;
            //Checks if the ship is within the bounds of the board, return subroutine early if it is.
            switch (ship.orientation)
            {
                case Ship.Orientation.Horizontal:

                    if (pos.x + ship.size - 1 > 7)
                    {
                        return false;
                    }
                    break;
                case Ship.Orientation.Vertical:
                    if (pos.y + ship.size - 1 > 7)
                    {
                        return false;
                    }
                    break;
            }
            //Check if where the ship is being placed is empty.
            return CheckForProperty(ship, Cell.Empty);

        }

        //CheckForProperty returns true if all spots where the ship would be are of property given in the parameter.
        private bool CheckForProperty(Ship ship, Cell property)
        {
            (int x, int y) pos = ship.position;
            switch (ship.orientation)
            {
                case Ship.Orientation.Horizontal:
                    for (int i = 0; i < ship.size; i++)
                    {
                        if (board[pos.x + i, pos.y] != property)
                        {
                            return false;
                        }
                    }
                    break;
                case Ship.Orientation.Vertical:
                    for (int i = 0; i < ship.size; i++)
                    {
                        if (board[pos.x, pos.y + i] != property)
                        {
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        //BakePlacement bakes the ship position onto the self board.
        private void BakePlacement(Ship ship)
        {
            switch (ship.orientation)
            {
                case Ship.Orientation.Horizontal:
                    for (int i = 0; i < ship.size; i++)
                    {
                        board[ship.position.x + i, ship.position.y] = Cell.Ship;
                    }
                    break;
                case Ship.Orientation.Vertical:
                    for (int i = 0; i < ship.size; i++)
                    {
                        board[ship.position.x, ship.position.y + i] = Cell.Ship;
                    }
                    break;
            }
        }

        //IncomingAttack returns true if a ship exists in the own board at the position in the parameter. 
        //It also changes the property of the Cell at the position, to either being a hit or a miss.
        public bool IncomingAttack((int x, int y) pos)
        {
            if (board[pos.x, pos.y] == Cell.Ship)
            {
                board[pos.x, pos.y] = Cell.Hit;
                SelfHit++;
                return true;
            }
            else
            {
                board[pos.x, pos.y] = Cell.Miss;
                SelfMiss++;
                return false;
            }
        }

        //Attack sends an attack to the player given in the parameter.
        //Given the attack success, it changes the tracker board and the TrackerHit/Miss variables accordingly
        public bool Attack(Player opponent)
        {
            Console.Clear();
            Display();
            //ChooseSpot is defined per player.
            (int x, int y) attackPos = ChooseSpot();
            bool attackSuccessful = opponent.IncomingAttack(attackPos);
            Cell cellState;

            if (attackSuccessful)
            {
                cellState = Cell.Hit;
            }
            else
            {
                cellState = Cell.Miss;
            }
            trackerBoard[attackPos.x, attackPos.y] = cellState;

            if (attackSuccessful)
            {
                TrackerHit++;
            }
            else
            {
                TrackerMiss++;
            }

            return attackSuccessful;
        }

        //IsWinner checks if the current player's TrackerHit variable matches up with the FleetTotalSize variable
        //If it does, true is returned and player is considered winner.
        public bool IsWinner()
        {
            if (TrackerHit == FleetTotalSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //CheckAttackPosition returns true if the trackerboard is empty at the given position.
        public bool CheckAttackPosition((int x, int y) pos)
        {
            return trackerBoard[pos.x, pos.y] == Cell.Empty ? true : false;
        }

        //Abstract subroutines to be defined. within different player types. 
        public abstract void PlaceShips();
        public abstract (int x, int y) ChooseSpot();
        public abstract void Display();

        public Renderer printer = new();
    }
    //Cell enum is used within the two boards to make some operations more understandable for humans (like me).
    public enum Cell
    {
        Empty = '~',
        Ship = 'S',
        Miss = 'O',
        Hit = 'X',
    }
    //GameState and PlayerData store critical information about their respective items for saving and loading.
    public struct GameState
    {
        public PlayerData player1Data;
        public PlayerData player2Data;
        public int turnCount;
        public List<int> notChosen;

    }
    public struct PlayerData
    {
        public Cell[,] board;
        public Cell[,] trackerBoard;
        public int TrackerHit, TrackerMiss;
        public int SelfHit, SelfMiss;
    }

    //The game class, that ties both players and basic game functions togerther.
    internal class Game
    {
        //SaveGame creates a GameState instance of the current GameState, and tries to serialize and write it to the file at the filepath given in the parameter.
        //Returns true if save is successful.
        public bool SaveGame(string filepath)
        {
            Console.Clear();
            printer.WriteLine("Saving... Please wait.", Renderer.FG.Green);
            GameState save = new()
            {
                player1Data = player1.GetPlayerData(),
                player2Data = player2.GetPlayerData(),
                turnCount = this.turnCount,
            };
            save.notChosen = player2.notChosen;
            try
            {
                string data = JsonConvert.SerializeObject(save, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                File.WriteAllText(filepath, data);
                return true;
            }
            catch
            {
                //something went wrong while saving, what does this mean???? idk hopefully it never happens lets just pray
                Environment.Exit(0);
                return false;
            }
        }
        //LoadGame tries to deserialise and load the game state given in the file in the filepath.
        //If the deserialisation fails, error message is shown to user and user is returned to the main menu.
        //If the file at filepath doesnt exist, a game save is created at that file, and the game is initiated.
        public bool LoadGame(string filepath)
        {
            if (File.Exists(filepath))
            {
                string data = File.ReadAllText(filepath);
                try
                {
                    GameState state = (GameState)JsonConvert.DeserializeObject(data, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                    player1.SetPlayerData(state.player1Data);
                    player2.SetPlayerData(state.player2Data);
                    player2.notChosen = state.notChosen;
                    turnCount = state.turnCount;
                    savepath = filepath;
                }
                catch
                {
                    printer.WriteLine("File is corrupted or is not a save file!", Renderer.FG.Red);
                    printer.WriteLine("Press any button to go to menu and try again.", Renderer.FG.Gray);
                    Console.ReadKey();
                    return false;
                }
            }
            else
            {
                Console.Clear();
                printer.WriteLine("File does not exist!",Renderer.FG.Red);
                printer.WriteLine("Creating a new game save under file!", Renderer.FG.Green);
                Thread.Sleep(1000);
                savepath = filepath;
                InitialiseGame();
            }
            return true;
        }

        //The constructor for a default game. The default game always saves to "defaultsave.txt"
        public Game()
        {
            InitialiseGame();
            Play();
        }

        //The constructor for trying to continue a game save at the given filepath
        //if loading the save fails, the game will not be played, and user will be returned to the main menu.
        public Game(string filepath)
        {
            if (LoadGame(filepath))
            {
                Play();
            }

           
        }
                
        //the filepath that SaveGame() saves to between each turn by default.
        string savepath = "defaultsave.txt";

        private int turnCount = 0;

        Renderer printer = new();
        Human player1 = new();
        Computer player2 = new();

        //Self explanatory.
        public void InitialiseGame()
        {
            player1.PlaceShips();
            player2.PlaceShips();
        }

        //GameOver displays correct winner message given the parameter. 
        public void GameOver(bool player1Won)
        {
            Console.Clear();
            if (player1Won)
            {
                printer.WriteLine("Human won!");
            }
            else
            {
                printer.WriteLine("Computer won!");
            }
            Thread.Sleep(1000);
            printer.WriteLine("Press any key to return to menu.", Renderer.FG.Gray);
            Console.ReadKey();
        }

        //the Play subroutine does turns of the game until either player is the winner.
        //The game is saved between each turn, so that if the player exits the program, they can continue their save from where they left off. 
        //When either player wins the game, the GameOver subroutine is called and afterwards the user returnns to the main menu. 

        //Whenever either player wins, the last turn isn't saved. I have left this in by choice as I think it proves as a useful tool to review games (and also to cheese the game if you lose :D).
        //Theoretically its an easy fix where a check is added before the first do while to check if either player is the winner, GameOver + return if so, and where the game is saved after the game over sequence before returning.
        public void Play()
        {
            do
            {
                while (player1.Attack(player2))
                {
                    if (player1.IsWinner())
                    {
                        GameOver(true);
                        return;
                    }
                }
                while (player2.Attack(player1))
                {
                    if (player2.IsWinner())
                    {
                        GameOver(false);
                        return;
                    }
                }
                turnCount++;
                SaveGame(savepath);
            } while (true); 

            //this while true will always be broken by a return at some point, as either user will always win.
            //using extra variables makes the code more confusing, so i have resorted to using a while true here even though I am usually iffy against them.
        }
    }
}
