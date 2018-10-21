using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Presentation
{
    class InputView
    {
        public virtual int AskForWidth()
        {
            Console.WriteLine("What should the width of the dungeon be? ");
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            } catch
            {
                Console.WriteLine("Width should be a number.");
                return AskForWidth();
            }
        }

        public virtual int AskForHeight()
        {
            Console.WriteLine("What should the height of the dungeon be? ");
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Height should be a number.");
                return AskForHeight();
            }
        }

        public virtual int AskForInput()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.T: return 1;
                    case ConsoleKey.H: return 2;
                    case ConsoleKey.C: return 3;
                    case ConsoleKey.G: return 4;
                    case ConsoleKey.F: return 5;
                    default:
                        Console.WriteLine("Invalid input!");
                        continue;
                }
            }
        }
    }
}
