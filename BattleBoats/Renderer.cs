using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBoats
{

    //This renderer was meant to be something more useful, with a bunch of cool features like printing being handled by another thread and updating instead of Console.Clear()'ing. However I ran out of time.
    //That is why some things may seem 'too' verbose, and some features might seem vaguely related to the project. I had a vision and it failed but all features that are below were still useful within project.
    public class Renderer
    {
        //Colour enum's so that humans can use the colours.
        public enum FG
        {
            Constant = 1,
            Black = 30,
            Red = 31,
            Green = 32,
            Yellow = 33,
            Blue = 34,
            Magenta = 35,
            Cyan = 36,
            White = 37,
            Gray = 90,
            BrightRed = 91,
            BrightGreen = 92,
            BrightYellow = 93,
            BrightBlue = 94,
            BrightMagenta = 95,
            BrightCyan = 96,
            BrightWhite = 97,
        }
        public enum BG
        {
            Constant = 1,
            Black = 40,
            Red = 41,
            Green = 42,
            Yellow = 43,
            Blue = 44,
            Magenta = 45,
            Cyan = 46,
            White = 47,
            Gray = 100,
            BrightRed = 101,
            BrightGreen = 102,
            BrightYellow = 103,
            BrightBlue = 104,
            BrightMagenta = 105,
            BrightCyan = 106,
            BrightWhite = 107,
        }
        //The values that FG.Constant and BG.Constant will take.
        private FG DefaultForeground = FG.White;
        private BG DefaultBackground = BG.Black;

        //for sketching and printing all at once.
        private string sketchPad;

        //Constructor sets values for DefaultForeground and DefaultBackground.
        public Renderer(FG DefaultForeground = FG.White, BG DefaultBackground = BG.Black)
            {
                Console.SetCursorPosition(0, 0);
                SetDefaultColor(DefaultForeground, DefaultBackground);
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                sketchPad = String.Empty;

            }
        
        //Self explanatory.
        public void SetDefaultColor(FG DefaultForeground = FG.White, BG DefaultBackround = BG.Black)
            {
                this.DefaultForeground = DefaultForeground;
                this.DefaultBackground = DefaultBackround;
            }

        //Prints the contents of the sketchPad string on the requested position.
        public void Draw((int x, int y) pos)
            {
                Console.SetCursorPosition(pos.x, pos.y);
                int i = 1;
                foreach (string line in sketchPad.Split("\n"))
                {
                    Console.Write(line);
                    Console.SetCursorPosition(pos.x, pos.y + i++);                
                }
                sketchPad = String.Empty;
            }

        //README:
        //The subroutines below are similar to each other so I will explain how they work here instead of each individually.
        //First two lines sets the colour values for the text. If either is constant, it is set to the respective Default...ground variable.
        //No the string is not black magic, the sequence uses ANSI escape codes add colour information to the string.
        //Google: "ANSI escape code" to learn more about it if it looks interesting.

        //Adds to sketchPad without line break.
        public void Sketch(string text, FG ForegroundColor = FG.Constant, BG BackgroundColor = BG.Constant)
            {
                ForegroundColor = ForegroundColor == FG.Constant ? this.DefaultForeground : ForegroundColor;
                BackgroundColor = BackgroundColor == BG.Constant ? this.DefaultBackground : BackgroundColor;

                sketchPad += $"\x1b[38;4;{(int)ForegroundColor}m\x1b[38;4;{(int)BackgroundColor}m{text}\x1b[0m";
            }

        //Adds to sketchPad with line break.
        public void SketchLine(string text, FG ForegroundColor = FG.Constant, BG BackgroundColor = BG.Constant)
            {
                ForegroundColor = ForegroundColor == FG.Constant ? this.DefaultForeground : ForegroundColor;
                BackgroundColor = BackgroundColor == BG.Constant ? this.DefaultBackground : BackgroundColor;

                sketchPad += $"\x1b[38;4;{(int)ForegroundColor}m\x1b[38;4;{(int)BackgroundColor}m{text}\x1b[0m\n";
            }

        //Writes to console with line break.
        public void WriteLine(string text, FG ForegroundColor = FG.Constant, BG BackgroundColor = BG.Constant)
            {
                ForegroundColor = ForegroundColor == FG.Constant ? this.DefaultForeground : ForegroundColor;
                BackgroundColor = BackgroundColor == BG.Constant ? this.DefaultBackground : BackgroundColor;

                Console.Write($"\x1b[38;4;{(int)ForegroundColor}m\x1b[38;4;{(int)BackgroundColor}m{text}\x1b[0m\n");
            }

        //Writes to console without line break.
        public void Write(string text, FG ForegroundColor = FG.Constant, BG BackgroundColor = BG.Constant)
            {
                ForegroundColor = ForegroundColor == FG.Constant ? this.DefaultForeground : ForegroundColor;
                BackgroundColor = BackgroundColor == BG.Constant ? this.DefaultBackground : BackgroundColor;

                Console.Write($"\x1b[38;4;{(int)ForegroundColor}m\x1b[38;4;{(int)BackgroundColor}m{text}\x1b[0m");             
            }                      
        
        
    }
}
