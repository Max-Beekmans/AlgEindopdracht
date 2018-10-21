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

        //temp var to run bfs just once
        private int _distance = 0;
        private String _str = null;
        private bool _talisman = false;
        private bool _handGrenade = false;
        private bool _compass = false;

        public bool IsTalismanOn
        {
            get
            {
                return _talisman;
            }
            set
            {
                _talisman = value;
                _distance = 0;
            }
        }

        public bool IsHandGrenadeOn
        {
            get { return _handGrenade; }
            set
            {
                _handGrenade = value;
            }
        }

        public bool IsCompassOn
        {
            get { return _compass; }
            set
            {
                _compass = value;
            }
        }

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
            for(int y = 0;  y < _game.Dungeon.ViewGrid.GetLength(1); y++)
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
                if (_distance == 0) {
                    _distance = _game.Dungeon.BreadthFirstSearch();
                }
                sb.Append("-> Talisman");
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append("De talisman licht op en fluistert dat het eindpunt " + _distance + " stappen ver weg is");
                sb.Append("\r\n");
            }

            if(IsHandGrenadeOn)
            {
                sb.Append("-> Handgranaat");
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append("De kerker schudt in zijn grondvesten, de tegenstander in een aangenzende hallway is vermorzeld!");
                sb.Append("\r\n");
                sb.Append("Een donderend geluid maakt duidelijk dat gedeeltes van de kerker zijn ingestort...");
                sb.Append("\r\n");
            }

            if(IsCompassOn)
            {
                sb.Append("-> Kompas");
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append("Je haalt het kompas uit je zak. Het trilt in je hand en projecteert in lichtgevende letters op de muur:");
                sb.Append("\r\n");
                sb.Append("\r\n");
                if (_str == null) {
                    _str = _game.Dungeon.Dijkstra();
                }
                sb.Append(_str);
            } else {
                this._str = null;
            }
        }

        private void PrintHorizontal(StringBuilder sb, int y)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(0); x++)
            {
                sb.Append(_game.Dungeon.ViewGrid[x, y]?.ToString() ?? "");
                sb.Append(_game.Dungeon.ViewGrid[x, y]?.East?.ToString() ?? "   ");
            }
            sb.Append("\r\n");
        }

        private void PrintVerticalLines(StringBuilder sb)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(0); x++)
            {
                sb.Append("  |     ");
            }
            sb.Append("\r\n");
        }
        private void PrintVerticalHalls(StringBuilder sb, int y)
        {
            for (int x = 0; x < _game.Dungeon.ViewGrid.GetLength(0); x++)
            {
                sb.Append(" " + (_game.Dungeon.ViewGrid[x, y]?.South?.ToString() ?? "   ") +"    ");
            }
            sb.Append("\r\n");
        }
    }
}
