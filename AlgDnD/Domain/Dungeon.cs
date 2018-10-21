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
        private int _roomCount = 0;
        private int _edgeCount = 0;
        public List<Hall> Halls = new List<Hall>();
        public List<Room> Rooms = new List<Room>();
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
            GetEdgesAndRooms();
        }

        public void InitializeGrid()
        {
            ViewGrid = new Room[Width, Height];
            for (int x = 0; x < ViewGrid.GetLength(0); x++) {
                for (int y = 0; y < ViewGrid.GetLength(1); y++) {
                    ViewGrid[x, y] = new Room(_roomCount);
                    _roomCount++;
                }
            }
        }

        public void Generate()
        {
            int startX = _random.Next(Width);
            int startY = _random.Next(Height);

            ViewGrid[startX, startY].IsStart = true;
            Start = ViewGrid[startX, startY];

            int endX = _random.Next(Width);
            while (endX == startX) {
                endX = _random.Next(Width);
            }

            int endY = _random.Next(Height);
            while (endY == startY) {
                endY = _random.Next(Height);
            }

            ViewGrid[endX, endY].IsEnd = true;
            End = ViewGrid[endX, endY];

            for (int x = 0; x < ViewGrid.GetLength(0); x++) {
                for (int y = 0; y < ViewGrid.GetLength(1); y++) {
                    //Width range check
                    if (x + 1 < ViewGrid.GetLength(0)) {
                        ViewGrid[x, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x + 1, y], "horizontal")[0];
                        _edgeCount++;
                        //We already have an undirected graph so we only need one edge between nodes to go either way.
                        //ViewGrid[x + 1, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x + 1, y], "horizontal")[1];
                    }
                    //Height range check
                    if (y + 1 < ViewGrid.GetLength(1)) {
                        ViewGrid[x, y] = LinkRooms(ViewGrid[x, y], ViewGrid[x, y + 1], "vertical")[0];
                        _edgeCount++;
                        //ViewGrid[x, y + 1] = LinkRooms(ViewGrid[x, y], ViewGrid[x, y + 1], "vertical")[1];
                    }
                }
            }
        }

        private Room[] LinkRooms(Room a, Room b, string orientation)
        {
            Room[] linkedRooms = new Room[2];
            Hall hall = new Hall(a, b, _edgeCount, _random.Next(10));
            if (orientation == "horizontal") {
                a.East = hall;
                b.West = hall;
                linkedRooms[0] = a;
                linkedRooms[1] = b;
            } else {
                a.South = hall;
                b.North = hall;
                linkedRooms[0] = a;
                linkedRooms[1] = b;
            }
            return linkedRooms;
        }

        public int BreadthFirstSearch()
        {
            Queue<Room> queue = new Queue<Room>();
            Queue<int> distQueue = new Queue<int>();
            HashSet<Room> visited = new HashSet<Room>();

            queue.Enqueue(Start);
            distQueue.Enqueue(0);

            while (queue.Count > 0) {
                Room room = queue.Dequeue();
                int distance = distQueue.Dequeue();
                room.IsVisited = true;
                visited.Add(room);

                if (room == End) {
                    return distance;
                }

                //Get all adjacent vertices and queue new way
                for (int i = 0; i < room.AdjacentEdges.Count; ++i) {
                    Hall edge = room.AdjacentEdges[i];
                    Room r = CheckHall(edge, room);
                    if (r != null && !visited.Contains(r)) {
                        visited.Add(r);
                        queue.Enqueue(r);
                        distQueue.Enqueue(distance + 1);
                    }
                }

                //Get all adjacent vertices and queue old way
                //Room north_end = CheckHall(room.North, room);
                //Room south_end = CheckHall(room.South, room);
                //Room west_end = CheckHall(room.West, room);
                //Room east_end = CheckHall(room.East, room);

                //if (north_end != null && !visited.Contains(north_end) && !queue.Contains(north_end)) {
                //    queue.Enqueue(north_end);
                //    distQueue.Enqueue(distance + 1);
                //}

                //if (south_end != null && !visited.Contains(south_end) && !queue.Contains(south_end)) {
                //    queue.Enqueue(south_end);
                //    distQueue.Enqueue(distance + 1);
                //}

                //if (west_end != null && !visited.Contains(west_end) && !queue.Contains(west_end)) {
                //    queue.Enqueue(west_end);
                //    distQueue.Enqueue(distance + 1);
                //}

                //if (east_end != null && !visited.Contains(east_end) && !queue.Contains(east_end)) {
                //    queue.Enqueue(east_end);
                //    distQueue.Enqueue(distance + 1);
                //}
            }

            return -1;
        }

        //return null if any of the params are null or if the hall is destroyed
        //return opposite end of the hall adjacent to the room
        private Room CheckHall(Hall h, Room r)
        {
            if (h == null || r == null || h.IsDestroyed)
                return null;

            if (h.enda == r) {
                return h.endb;
            }
            return h.enda;
        }

        //for union by rank (optimization)
        struct Subset
        {
            public int parent;
            public int rank;
        }

        public void Kruskal()
        {
            if (Rooms == null || Halls == null)
                GetEdgesAndRooms();
            //sort edges(halls) by weight (enemy)
            List<Hall> sorted_edges = Halls.OrderBy(o => o.Enemy).ToList();
            Subset[] subsets = new Subset[_roomCount];
            //If necessary?
            Dictionary<int, int> ids = new Dictionary<int, int>();
            
            for (int i = 0; i < _roomCount; ++i) {

                subsets[i].parent = i;
                subsets[i].rank = 0;
                //map indices to roomid
                ids.Add(Rooms[i].Id, i);
            }

            for (int j = 0; j < _edgeCount; ++j) {
                int ia = ids[Halls[j].enda.Id];
                int ib = ids[Halls[j].endb.Id];
                int x = Find(subsets, Halls[j].enda.Id);
                int y = Find(subsets, Halls[j].endb.Id);

                if (x != y) {
                    Union(subsets, x, y);
                } else {
                    Halls[j].IsDestroyed = true;
                }
            }
            UpdateEdges();
        }

        private int Find(Subset[] subset, int i)
        {
            // find root and make root as parent of i (path compression)
            if (subset[i].parent != i) {
                subset[i].parent = Find(subset, subset[i].parent);
            }
            return subset[i].parent;
        }

        private void Union(Subset[] subset, int x, int y)
        {
            int set1 = Find(subset, x);
            int set2 = Find(subset, y);
            //If the two sets aren't connected unify them
            if (set1 != set2) {
                subset[set1].parent = set2;
            }
        }

        private void GetEdgesAndRooms()
        {
            //get all edges
            //using bfs like travesal?
            Queue<Room> queue = new Queue<Room>();
            List<Room> visited = new List<Room>();
            List<Hall> edges = new List<Hall>();
            List<Room> rooms = new List<Room>();

            queue.Enqueue(Start);
            while (queue.Count > 0) {
                Room r = queue.Dequeue();
                visited.Add(r);
                rooms.Add(r);
                for (int i = 0; i < r.AdjacentEdges.Count; ++i) {
                    Hall edge = r.AdjacentEdges[i];
                    edges.Add(edge);
                    Room other = CheckHall(edge, r);
                    if (other != null && !visited.Contains(other)) {
                        visited.Add(other);
                        queue.Enqueue(other);
                    }
                }
            }

            this.Halls = edges;
            this.Rooms = rooms;
            this._edgeCount = edges.Count;
            this._roomCount = rooms.Count;
        }

        private void UpdateEdges()
        {
            //get all edges
            //using bfs like travesal?
            Queue<Room> queue = new Queue<Room>();
            List<Room> visited = new List<Room>();

            queue.Enqueue(Start);
            while (queue.Count > 0) {
                Room r = queue.Dequeue();
                visited.Add(r);
                for (int i = 0; i < r.AdjacentEdges.Count; ++i) {
                    Hall obj = Halls.Find(o => o.Id == r.AdjacentEdges[i].Id);
                    r.AdjacentEdges[i] = obj;
                    Room other = CheckHall(obj, r);
                    if (other != null && !visited.Contains(other)) {
                        visited.Add(other);
                        queue.Enqueue(other);
                    }
                }
            }
        }
    }
}
