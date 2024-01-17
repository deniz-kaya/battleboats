using System.ComponentModel.Design;
using System.Drawing;

namespace BattleBoats
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            //Creating instances of required classes.
            Game Game;
            UI Interface = new();

            bool quit = false;
            while (!quit)
            {
                //This displays a new menu, and sets the request to the option that the user has requested through the menu
                UI.Request request = Interface.Menu();
                switch (request)
                {
                    //These are either very self explanatory, or the relevant subroutine explanations are elsewhere. Look through UI.cs and Game.cs.
                    case UI.Request.NewQuickGame:
                        Game = new();
                        break;
                    case UI.Request.ResumeQuickGame:
                        Game = new("defaultsave.txt");
                        break;
                    case UI.Request.LoadGame:
                        Game = new(Interface.GetFilepath());
                        break;
                    case UI.Request.Instructions:
                        Interface.Instructions();
                        break;
                    case UI.Request.Quit:
                        quit = true;
                        break;
                }
            }
            Environment.Exit(0);
        }
    }
}

