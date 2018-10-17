using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AlgDnD.Domain
{
    public class Game
    {
        private Dungeon _dungeon;

        public event EventHandler GameUpdated;

        private Thread _gameThread;

        private bool _running;

        public Dungeon Dungeon
        {
            get { return _dungeon; }
        }

        public void Start()
        {
            if (_gameThread != null) _gameThread.Abort();
            _gameThread = new Thread(GameCycle);
            _gameThread.Start();
            _running = true;
        }

        public void GameCycle()
        {
            while (_running)
            {
                //Game loop

                GameUpdated?.Invoke(this, EventArgs.Empty);
                Thread.Sleep(50);
            }
        }

        public void CreateDungeon(int width, int height)
        {
            _dungeon = new Dungeon(width, height);
        }



    }
}
