using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD.Domain
{
    public class Room
    {
        private List<Hall> _adjacentEdges;
        private bool _isBegin = false;
        private bool _isEnd = false;
        private bool _visited = false;
        
        public Room()
        {
            _adjacentEdges = new List<Hall>();
        }

        public void AddEdge(Hall edge)
        {
            if (edge != null) {
                _adjacentEdges.Add(edge);
            }
        }

        public void DeleteEdge(Hall edge)
        {
            //have some min spanning tree stuff here probably?
            if (edge != null && _adjacentEdges.Count > 1) {
                _adjacentEdges.Remove(edge);
            }
        }

        public void AddRoomConnection(Room a)
        {
            Hall connection = new Hall(this, a, 6);
            this.AddEdge(connection);
            a.AddEdge(connection);
        }
    }
}
