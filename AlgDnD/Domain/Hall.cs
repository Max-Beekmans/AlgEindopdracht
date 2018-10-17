using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Hall
    {
        //undirected graph (you can go back anywhere)
        //a,b == b,a
        //a -- b
        //directed graph (you can only go along the direction of the edge)
        //a,b != b,a
        //a -> b
        public Room enda = null;
        public Room endb = null;
        public int Enemy = 0;
        public bool IsDestroyed { get; set; }
        public bool IsVertical { get; set; }

        public Hall(Room a, Room b, int enemy = 0)
        {
            if (a != null && b != null) {
                enda = a;
                endb = b;
                Enemy = enemy;
            }
        }

        public override string ToString()
        {
            return IsDestroyed ? " ~ " : " "+Enemy+" ";
        }
    }
}
