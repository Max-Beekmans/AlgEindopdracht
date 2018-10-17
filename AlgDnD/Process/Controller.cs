using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AlgDnD.Domain;
using AlgDnD.Presentation;

namespace AlgDnD.Process
{
    public class Controller
    {
        private Game _game;

        private Thread _inputThread;

        private OutputView _outputview;
        private InputView _inputView;

        private bool _running;

        public Controller()
        {
            _outputview = new OutputView();
            _inputView = new InputView();
            _inputThread = new Thread(ParseInput);
        }

        public virtual void Run()
        {
            _running = true;
            _outputview.ShowWelcome();
            int width = _inputView.AskForWidth();
            int height = _inputView.AskForHeight();
            _game = new Game();
            _game.CreateDungeon(width, height);
            _outputview.Game = _game;
            _outputview.DrawDungeon();
            _game.Start();
            _outputview.RegisterToEvents();
            _inputThread.Start();
        }

        public void ParseInput()
        {
            while (_running)
            {
                int input = _inputView.AskForInput();
                switch (input)
                {
                    case 1:
                        _outputview.IsTalismanOn = !_outputview.IsTalismanOn;
                        break;
                    case 2:
                            
                        break;
                    case 3:
                            
                        break;
                    case 4:
                        _game.Dungeon.InitializeGrid();
                        _game.Dungeon.Generate();
                        break;
                }
            }
        }
    }
}
