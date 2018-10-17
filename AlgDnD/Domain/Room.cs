using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Room
    {
        //private List<Hall> _adjacentEdges;

        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsVisited { get; set; }

        public Hall North { get; set; }
        public Hall South { get; set; }
        public Hall West { get; set; }
        public Hall East { get; set; }

        public int Id { get; set; }


        public Room(int id)
        {
            Id = id;
            North = null;
            South = null;
            West = null;
            East = null;
        }

        public override string ToString()
        {
            return IsStart ? "- S -" : (IsEnd ? "- E -" : (IsVisited ? "- * -" : "- X -"));
        }
    }
}
