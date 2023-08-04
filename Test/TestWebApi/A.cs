namespace TestWebApi
{
    public class A
    {
        public static List<Point3D> AStar(Graph graph, Point3D start, Point3D goal)
        {
            var openSet = new List<Point3D> { start };
            var cameFrom = new Dictionary<Point3D, Point3D>();
            var gScore = new Dictionary<Point3D, double> { [start] = 0 };
            var fScore = new Dictionary<Point3D, double> { [start] = Heuristic(start, goal) };

            while (openSet.Count > 0)
            {
                var current = openSet.OrderBy(n => fScore[n]).First();

                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                foreach (var neighbor in graph.GetNodes())
                {
                    var tentativeGScore = gScore[current] + Distance(current, neighbor);

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private static double Heuristic(Point3D a, Point3D b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }

        private static double Distance(Point3D a, Point3D b)
        {
            return Heuristic(a, b);
        }

        private static List<Point3D> ReconstructPath(Dictionary<Point3D, Point3D> cameFrom, Point3D current)
        {
            var path = new List<Point3D> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }

    }
}
