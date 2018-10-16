using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Dungeon
    {
        private Random _random;
        public Room[,] ViewGrid { get; set; }
        List<Room> genVisited = new List<Room>();

        public Room Start { get; set; }
        public Room End { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Dungeon(int width, int height)
        {
            //generate dungeon with dimensions
            Width = width;
            Height = height;
            _random = new Random();
            InitializeGrid();
            Generate();
        }

        private void InitializeGrid()
        {
            ViewGrid = new Room[Width, Height];
            for (int y = 0; y < ViewGrid.GetLength(0); y++)
            {
                for (int x = 0; x < ViewGrid.GetLength(1); x++)
                {
                    ViewGrid[x, y] = new Room((x + 1) * (y + 1));
                }
            }
        }

        private void Generate()
        { 
            int startX = _random.Next(Width);
            int startY = _random.Next(Height);

            ViewGrid[startX, startY].IsStart = true;

            int endX = _random.Next(Width);
            while(endX == startX)
            {
                endX = _random.Next(Width);
            }

            int endY = _random.Next(Height);
            while (endY == startY)
            {
                endY = _random.Next(Height);
            }

            ViewGrid[endX, endY].IsEnd = true;

            for(int y = 0; y < ViewGrid.GetLength(0); y++)
            {
                for(int x = 0; x < ViewGrid.GetLength(1); x++)
                {
                    if (x+1 < ViewGrid.GetLength(1))
                    {
                        ViewGrid[x, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x + 1, y], "horizontal")[0];
                        ViewGrid[x + 1, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x + 1, y], "horizontal")[1];
                    }

                    if(y+1 < ViewGrid.GetLength(0))
                    {
                        ViewGrid[x, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x, y + 1], "vertical")[0];
                        ViewGrid[x, y + 1] = LinkRooms(ViewGrid[x, y], ViewGrid[x, y + 1], "vertical")[1];
                    }
                }
            }
        }

        private Room[] LinkRooms(Room a, Room b, string orientation)
        {
            Room[] linkedRooms = new Room[2];
            Hall hall = new Hall(a, b, _random.Next(10));
            if(orientation == "horizontal")
            {
                a.East = hall;
                b.West = hall;
                linkedRooms[0] = a;
                linkedRooms[1] = b;
            } else
            {
                a.South = hall;
                b.North = hall;
                linkedRooms[0] = a;
                linkedRooms[1] = b;
            }
            return linkedRooms;
        }
    }
}
