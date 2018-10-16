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
            Console.WriteLine("DUNGEON - " + _game.Dungeon.Width + " x " + _game.Dungeon.Height);

            _drawing = false;
        }
    }
}
