using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Room
    {
        public List<Hall> AdjacentEdges { get; set; }

        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsVisited { get; set; }
        public int Distance { get; set; }

        public Hall North
        {
            get
            {
                return _north;
            }
            set
            {
                _north = value;
                if (value != null) {
                    AdjacentEdges.Add(value);
                }
            }
        }
        public Hall South
        {
            get
            {
                return _south;
            }
            set
            {
                _south = value;
                if (value != null) {
                    AdjacentEdges.Add(value);
                }
            }
        }
        public Hall West
        {
            get { return _west; }
            set { _west = value; if (value != null) { AdjacentEdges.Add(value); } }
        }
        public Hall East
        {
            get { return _east; }
            set { _east = value; if (value != null) { AdjacentEdges.Add(value); } }
        }

        private Hall _north = null;
        private Hall _south = null;
        private Hall _west = null;
        private Hall _east = null;

        public int Id { get; set; }

        public Room(int id)
        {
            Id = id;
            AdjacentEdges = new List<Hall>();
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
