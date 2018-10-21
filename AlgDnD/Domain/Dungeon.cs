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
        private bool inuse = false;
        private Random _random;
        private int _roomCount = 0;
        private int _edgeCount = 0;
        public List<Hall> shortest_path = new List<Hall>();
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
            _roomCount = 0;
            for (int x = 0; x < ViewGrid.GetLength(0); x++) {
                for (int y = 0; y < ViewGrid.GetLength(1); y++) {
                    ViewGrid[x, y] = new Room(_roomCount);
                    ViewGrid[x, y].Id = _roomCount;
                    _roomCount++;
                }
            }
        }

        public void Generate()
        {
            _edgeCount = 0;
            Rooms.Clear();
            Halls.Clear();
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

        public void ChangeWeight()
        {
            if (shortest_path != null && shortest_path.Count > 1) {
                int index = _random.Next(shortest_path.Count);
                Hall temp = shortest_path[index];
                Hall obj = Halls.Find(o => o.Id == temp.Id);
                obj.Enemy = (obj.Enemy * 2);
                UpdateEdges();
            } else {
                Dijkstra();
                ChangeWeight();
            }
            //terminate since we don't have a shortest_path yet
        }

        //Breadth First Search starts at the starting node and goes past every adjacent edge.
        //If the adjacent room hasn't been visited it will be added to the queue
        //Then a new room will be picked from the queue until the queue is eventually empty and we have visited all the reachable vertices in the graph.
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
            }

            return -1;
        }

        public string Dijkstra()
        {
            if (inuse) {
                return "already in use";
            }
            shortest_path.Clear();
            HashSet<Room> unvisited = new HashSet<Room>(Rooms);
            //distance with room id as key
            int[] distance = new int[_roomCount];
            Room currentRoom = null;
            Room prevRoom = null;
            string directionString = "";
            string enemyString = "";
            int enemyCount = 0;

            //set distance to infinity except for start room
            foreach (Room room in unvisited) {
                if (room.Id == Start.Id) {
                    distance[room.Id] = 0;
                    currentRoom = room;
                } else {
                    distance[room.Id] = int.MaxValue;
                }
            }

            while (unvisited.Count > 0) {
                //update the distance for all neighbours with lower value if possible
                //TODO if it's not in shortest path?
                foreach (Hall hall in currentRoom.AdjacentEdges) {
                    Room r = CheckHall(hall, currentRoom);
                    int newDistance = distance[currentRoom.Id] + hall.Enemy;
                    if (r != null && unvisited.Contains(r) && newDistance < distance[r.Id] && !shortest_path.Contains(hall)) {
                        distance[r.Id] = newDistance;
                    }
                }

                unvisited.Remove(currentRoom);

                if (currentRoom.Id != End.Id) {
                    //ordered list on enemy so First() will give the least hard path
                    List<Hall> halls = currentRoom.AdjacentEdges.OrderBy(h => distance[CheckHall(h, currentRoom).Id]).ToList();
                    //List<Hall> halls = currentRoom.AdjacentEdges.OrderBy(h => h.Enemy).ToList();
                    Room nextRoom = null;
                    Hall nextHall = null;

                    int i = 0;
                    while (nextRoom == null) {
                        Room other = CheckHall(halls[i], currentRoom);
                        if (other != null && unvisited.Contains(other) && distance[other.Id] < distance[currentRoom.Id]) {
                            nextRoom = other;
                            nextHall = halls[i];
                        }
                        i++;
                    }

                    if (nextRoom != null && nextHall != null) {
                        if (nextHall == currentRoom?.North) {
                            directionString += "-> Noord";
                        } else if (nextHall == currentRoom?.South) {
                            directionString += "-> Zuid";
                        } else if (nextHall == currentRoom?.West) {
                            directionString += "-> West";
                        } else if (nextHall == currentRoom?.East) {
                            directionString += "-> Oost";
                        }

                        if (nextHall.Enemy > 0) {
                            enemyString += "level " + nextHall.Enemy + ",";
                            enemyCount++;
                        }

                        prevRoom = currentRoom;
                        currentRoom = nextRoom;
                        shortest_path.Add(nextHall);
                    }

                } else {
                    unvisited.Clear();
                }
            }

            string message = "";
            message += directionString;
            message += "\r\n\r\n";
            message += enemyCount + " tegenstanders (";
            message += enemyString + ")";

            return message;
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

        //Kruskals min span algorithm
        //1. Take all edges from the tree and sort them based on weight. (or in our case Enemy)
        //2. Loop through edges and find the subset of either end of the edge.
        //3. If they aren't in the same subset unify them by rank.
        //4. If they are it means this edge we want to add will make a cycle. We don't like cycles. So we destroy that edge.
        //5. Update the edges
        public void Kruskal()
        {
            if (Rooms == null || Halls == null || Rooms.Count < 1 || Halls.Count < 1)
                GetEdgesAndRooms();
            //sort edges(halls) by weight (enemy)
            List<Hall> sorted_edges = Halls.OrderBy(o => o.Enemy).ToList();
            Subset[] subsets = new Subset[_roomCount];
            //If necessary?
            Dictionary<int, int> ids = new Dictionary<int, int>();
            
            //init subset and ids dictonary
            for (int i = 0; i < _roomCount; ++i) {

                subsets[i].parent = i;
                subsets[i].rank = 0;
                //map indices to roomid
                ids.Add(Rooms[i].Id, i);
            }

            //loop through edges
            for (int j = 0; j < _edgeCount; ++j) {
                int ia = ids[sorted_edges[j].enda.Id];
                int ib = ids[sorted_edges[j].endb.Id];
                int x = Find(subsets, ia);
                int y = Find(subsets, ib);

                if (x != y) {
                    Union(subsets, x, y);
                } else {
                    sorted_edges[j].IsDestroyed = true;
                }
            }
            Halls = sorted_edges;
            UpdateEdges();
        }

        //Find (Path compression opt)
        //finds the subsets of either elements and compares if they are equal.
        //if they aren't we recursively set the root as parent of i
        //this further reduces the amount of traversals
        private int Find(Subset[] subset, int i)
        {
            // find root and make root as parent of i (path compression)
            if (subset[i].parent != i) {
                subset[i].parent = Find(subset, subset[i].parent);
            }
            return subset[i].parent;
        }

        //UnionByRank that attaches the smaller rank tree under the root of the high rank tree
        //If the ranks are equal a new root note is assigned and it's rank is incremented
        private void Union(Subset[] subset, int x, int y)
        {
            int set1 = Find(subset, x);
            int set2 = Find(subset, y);
            // Attach Smaller rank tree under root of high rank tree
            // (Union by Rank)
            if (subset[set1].rank < subset[set2].rank) {
                subset[set1].parent = set2;
            } else if (subset[set1].rank > subset[set2].rank) {
                subset[set2].parent = set1;
            } else {
                subset[set2].parent = set1;
                subset[set1].rank++;
            }
        }
        //BFS through the tree and collect all rooms and edges in lists.
        //Also updates edge and room count
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
                    if (!edges.Contains(edge)) {
                        edges.Add(edge);
                    }
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

        //BFS through the tree and sets all edges to it's copy in the hall list
        //I know it's a clunky way to update a datastructure like this couse of the find operation on list.
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
