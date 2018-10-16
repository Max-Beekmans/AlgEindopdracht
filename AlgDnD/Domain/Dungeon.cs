using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Dungeon
    {
        private Room _begin = null;
        private Room _end = null;
        private int _width;
        private int _height;

        public Room Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }

        public Room End
        {
            get { return _end; }
            set { _end = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Dungeon()
        {
            makeSampleDungeon();
        }

        public Dungeon(int width, int height)
        {
            //generate dungeon with dimensions
            _width = width;
            _height = height;

            makeSampleDungeon();
        }

        private void makeSampleDungeon()
        {
            // 1   2   3   4
            // O - O - O - O
            // |       |
            // O -- -- O
            // 5       6
            Room room1 = new Room();
            Room room2 = new Room();
            Room room3 = new Room();
            Room room4 = new Room();
            //Room room5 = new Room();
            //Room room6 = new Room();

            //room1.AddRoomConnection(room5);
            room1.AddRoomConnection(room2);
            room2.AddRoomConnection(room3);
            room3.AddRoomConnection(room4);
            //room3.AddRoomConnection(room6);
            //room5.AddRoomConnection(room6);

            _begin = room1;
            _end = room4;
        }
    }
}
