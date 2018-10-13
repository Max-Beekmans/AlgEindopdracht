using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD
{
    public class Room
    {
        List<Hall> adjacentEdges;

        public Room()
        {
            adjacentEdges = new List<Hall>();
        }

        public void AddVertice(Hall vertice)
        {
            if (vertice != null) {
                adjacentEdges.Add(vertice);
            }
        }

        public void DeleteVertice(Hall vertice)
        {
            //have some min spanning tree stuff here probably?
            if (vertice != null && adjacentEdges.Count > 1) {
                adjacentEdges.Remove(vertice);
            }
        }

        public void AddRoomConnection(Room a)
        {
            Hall connection = new Hall(this, a, 6);
            this.AddVertice(connection);
            a.AddVertice(connection);
        }
    }
}
