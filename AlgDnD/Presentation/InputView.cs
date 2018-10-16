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
    }
}
