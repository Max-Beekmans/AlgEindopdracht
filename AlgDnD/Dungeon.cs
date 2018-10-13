using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD
{
    public class Dungeon
    {
        Room begin = null;
        Room end = null;
        
        public Dungeon()
        {
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
            Room room5 = new Room();
            Room room6 = new Room();

            room1.AddRoomConnection(room5);
            room1.AddRoomConnection(room2);
            room2.AddRoomConnection(room3);
            room3.AddRoomConnection(room4);
            room3.AddRoomConnection(room6);
            room5.AddRoomConnection(room6);

            begin = room1;
            end = room4;
        }
    }
}
