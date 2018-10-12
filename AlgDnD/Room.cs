using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgDnD
{
    public class Room
    {
        List<Hall> adjacentVertices;

        public Room()
        {
            adjacentVertices = new List<Hall>();
        }

        public void AddVertice(Hall vertice)
        {
            if (vertice != null) {
                adjacentVertices.Add(vertice);
            }
        }

        public void DeleteVertice(Hall vertice)
        {
            //have some min spanning tree stuff here probably?
            if (vertice != null && adjacentVertices.Count > 1) {
                adjacentVertices.Remove(vertice);
            }
        }
    }
}
