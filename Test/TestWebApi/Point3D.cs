using Consul;

namespace TestWebApi
{
    // Graph.cs
    public class Graph
    {
        private List<Point3D> nodes;
        private List<Edge> edges;

        public Graph(List<Point3D> nodes, List<Edge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public List<Point3D> GetNodes()
        {
            return nodes;
        }

        public List<Edge> GetEdges()
        {
            return edges;
        }
    }

    // Point3D.cs
    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public class Edge
    {
        public Point3D StartNode { get; set; }
        public Point3D EndNode { get; set; }
        public double Weight { get; set; }

        public Edge(Point3D startNode, Point3D endNode, double weight)
        {
            StartNode = startNode;
            EndNode = endNode;
            Weight = weight;
        }
    }
}