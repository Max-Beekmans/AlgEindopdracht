using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgDnD.Domain;

namespace AlgDnD.Presentation
{
    class OutputView
    {
        private Game _game;
        private bool _drawing;
        
        public bool IsTalismanOn { get; set; }

        public Game Game
        {
            get { return _game; }
            set
            {
                _game = value;
            }
        }

        public void ShowWelcome()
        {
            Console.Clear();
            Console.Title = "RogueLike";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(AlgDnD.Properties.Resources.WelcomeMessage);

            Console.ReadKey(true);
        }

        public void RegisterToEvents()
        {
            _game.GameUpdated += OnDungeonUpdated;
        }

        public void OnDungeonUpdated(object sender, EventArgs eArgs)
        {
            if (!_drawing)
            {
                DrawDungeon();
            }
        }

        public void DrawTitle()
        {
            if(_game.Dungeon.Width > 0 && _game.Dungeon.Height > 0)
            {
                Console.Title = "RogueLike - Size: " + _game.Dungeon.Width + " x " + _game.Dungeon.Height;
            } else
            {
                Console.Title = "RogueLike";
            }
            
        }

        public void DrawDungeon()
        {
            _drawing = true;
            Console.Clear();
            DrawTitle();

            StringBuilder sb = new StringBuilder();
            sb.Append(AlgDnD.Properties.Resources.Header);

            //Loop through graph and print the rooms and halls
            for(int y = 0;  y < _game.Dungeon.ViewGrid.GetLength(0); y++)
            {
                PrintVerticalLines(sb);
                PrintHorizontal(sb, y);
                PrintVerticalLines(sb);
                PrintVerticalHalls(sb, y);
            }
            PrintMessage(sb);
            Console.Write(sb);
            _drawing = false;
        }

        public void PrintMessage(StringBuilder sb)
        {
            if(IsTalismanOn)
            {
                sb.Append("De talisman licht op en fluistert dat het eindpunt " + _game.Dungeon.BreadthFirstSearch() + " stappen ver weg is");
                sb.Append("\r\n");
            }
        }

        private void PrintHorizontal(StringBuilder sb, int y)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(1); x++)
            {
                sb.Append(_game.Dungeon.ViewGrid[x, y]?.ToString() ?? "");
                sb.Append(_game.Dungeon.ViewGrid[x, y]?.East?.ToString() ?? "   ");
            }
            sb.Append("\r\n");
        }

        private void PrintVerticalLines(StringBuilder sb)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(1); x++)
            {
                sb.Append("  |     ");
            }
            sb.Append("\r\n");
        }
        private void PrintVerticalHalls(StringBuilder sb, int y)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(1); x++)
            {
                sb.Append(" " + (_game.Dungeon.ViewGrid[x, y]?.South?.ToString() ?? "   ") +"    ");
            }
            sb.Append("\r\n");
        }
    }
}
