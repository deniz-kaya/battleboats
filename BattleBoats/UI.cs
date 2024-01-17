using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class UI
    {
        //Used within Menu()
        public enum Request
        {
            NewQuickGame = 1,
            ResumeQuickGame = 2,
            LoadGame = 3,
            Instructions = 4,
            Quit = 5,
        }

        //Constructor can include custom foreground and background values for custom user interface. If none are included constant values are assumed.
        public UI(Renderer.FG foreground = Renderer.FG.Constant, Renderer.BG background = Renderer.BG.Constant)
        {
            printer = new(foreground, background);
        }
        private Renderer.Printer printer = new();

        //Print a whole chunk of text, instructions.
        public void Instructions()
        {            
            printer.Sketch("BattleBoats is a turn-based strategy game where players eliminate their\nopponent's fleet of boats by ‘firing’ at a location on a grid in an attempt to sink them.\n");
            printer.Sketch("The first player to sink all of their opponents’ battle boats is declared the winner.\n");
            printer.Sketch("\n");
            printer.Sketch("Each player has two eight by eight grids.\n");
            printer.Sketch("One grid is used for their own battle boats and the other is used to record any hits or misses placed on their opponents.\n");
            printer.Sketch("At the beginning of the game, players decide where they wish to place their fleet of five battle boats.\n", Renderer.FG.Green);
            printer.Sketch("\n");
            printer.Sketch("During gameplay, players take turns to fire at a location on their opponent’s board.\n",Renderer.FG.Green);
            printer.Sketch("They do this by stating the coordinates for their target.\n");
            printer.Sketch("If a player hits their opponent's boat then this is recorded as a hit.\n",Renderer.FG.Green);
            printer.Sketch("If they miss then this is recorded as a miss.\n");
            printer.Sketch("\n");
            printer.Sketch("The game ends when a player's fleet of boats has been sunk.\n",Renderer.FG.Green);
            printer.Sketch("The winner is the player with boats remaining at the end of the game\n");
            printer.Sketch("\n");
            printer.Sketch("Press any key to return back to the main menu.", Renderer.FG.Green);
            Console.Clear();
            printer.Draw((0, 0));
            Console.ReadKey();
        }
        
        //Selt explanatory.
        public string GetFilepath()
        {
            Console.Clear();
            printer.WriteLine("Please input the filepath of the game: ");
            printer.Write("> ");
            return Console.ReadLine();
        }

        //Display menu and return request made by the user.
        public Request Menu()
        {
            Console.Clear();
            //big text
            printer.SketchLine(" ____        _   _   _      ____              _       ");
            printer.SketchLine("|  _ \\      | | | | | |    |  _ \\            | |      ");
            printer.SketchLine("| |_) | __ _| |_| |_| | ___| |_) | ___   __ _| |_ ___ ");
            printer.SketchLine("|  _ < / _` | __| __| |/ _ |  _ < / _ \\ / _` | __/ __|");
            printer.SketchLine("| |_) | (_| | |_| |_| |  __| |_) | (_) | (_| | |_\\__ \\");
            printer.SketchLine("|____/ \\__,_|\\__|\\__|_|\\___|____/ \\___/ \\__,_|\\__|___/");

            //boat
            printer.SketchLine("            ,:',:`,:'", Renderer.FG.Gray);
            printer.SketchLine("         __||_||_||_||__", Renderer.FG.Red);
            printer.Sketch("    ____[", Renderer.FG.Red);
            printer.Sketch("\"\"\"\"\"\"\"\"\"\"\"\"\"\"\""); 
            printer.SketchLine("]____", Renderer.FG.Red);
            printer.Sketch("    \\ ", Renderer.FG.Red);
            printer.Sketch("\" '''''''''''''''''''' ");
            printer.SketchLine("| ", Renderer.FG.Red); 
            printer.SketchLine("~~~^~^~^^~^~^~^~^~^~^~^~~^~^~^^~~^~^", Renderer.FG.BrightBlue);

            //menu
            printer.SketchLine("1 - New Quick Game");
            printer.SketchLine("2 - Resume Quick Game");
            printer.SketchLine("3 - Load/Create Save");
            printer.SketchLine("4 - Instructions");
            printer.SketchLine("5 - Quit");
            printer.Draw((0,0));

            string ans;
            bool validOption = true;
            int input = 1;
            //Until it is a valid request, keep asking user to enter requestsl
            do
            {                
                if (!validOption)
                {
                    printer.WriteLine("\nInvalid input!",Renderer.FG.BrightRed);
                }
                printer.Write("Input request (1-5): ");

                ans = Console.ReadLine();

                if (ans.Length == 1 && Char.IsNumber(ans[0]) && int.Parse(ans) > 0 && int.Parse(ans) < 6)
                {
                    input = int.Parse(ans);
                    validOption = true;
                }
                else 
                { 
                    validOption = false; 
                }

            } while (!validOption);

            return (Request)input;
        }
    }
}
