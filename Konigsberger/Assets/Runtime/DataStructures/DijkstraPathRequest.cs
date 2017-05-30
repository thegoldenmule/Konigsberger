using System.Collections.Generic;

public class DijkstraPathRequest
{
    private class CostQueue
    {
        private readonly float[] _costs;

        private readonly List<int> _sortedIndices = new List<int>();

        public bool IsEmpty
        {
            get
            {
                return 0 == _sortedIndices.Count;
            }
        }

        public CostQueue(ref float[] costs)
        {
            _costs = costs;
        }
        
        public void Insert(int index)
        {
            float cost = _costs[index];
            for (int i = 0, len = _sortedIndices.Count; i < len; i++)
            {
                if (cost < _costs[i])
                {
                    _sortedIndices.Insert(i, index);
                    return;
                }
            }

            _sortedIndices.Add(index);
        }

        public int Pop()
        {
            if (IsEmpty)
            {
                return 0;
            }

            int index = _sortedIndices[0];
            _sortedIndices.RemoveAt(0);
            return index;
        }

        public void UpdatePriority(int index)
        {
            _sortedIndices.Remove(index);

            Insert(index);
        }
    }

    private CostQueue _priorityQueue;
    private float[] _costToNode;
    private GraphEdge[] _searchFrontier;
    private GraphEdge[] _shortestPathTree;
    private GraphNode[] _path;

    public delegate float Heuristic(GraphNode a, GraphNode b);

    public Graph Graph
    {
        get;
        private set;
    }

    public GraphNode Start
    {
        get;
        private set;
    }

    public GraphNode End
    {
        get;
        private set;
    }

    public bool Success
    {
        get;
        private set;
    }

    public GraphNode[] Path
    {
        get
        {
            if (null == _path && Success)
            {
                List<GraphNode> nodes = new List<GraphNode>();

                int index = End.Index;
                while (index != Start.Index)
                {
                    nodes.Insert(0, Graph.GetNode(index));

                    index = _shortestPathTree[index].From;
                }

                nodes.Insert(0, Start);

                _path = nodes.ToArray();
            }

            return _path;
        }
    }

    public float MaxCost { get; set; }

    public DijkstraPathRequest Search(
        Graph graph,
        GraphNode start,
        GraphNode end,
        Heuristic heuristic)
    {
        Graph = graph;
        Start = start;
        End = end;

        Success = false;
        _path = null;

        _costToNode = new float[graph.NumNodes];
        _searchFrontier = new GraphEdge[graph.NumNodes];
        _shortestPathTree = new GraphEdge[graph.NumNodes];
        _priorityQueue = new CostQueue(ref _costToNode);

        _priorityQueue.Insert(start.Index);

        bool limitCost = MaxCost > 0f;

        while (!_priorityQueue.IsEmpty)
        {
            int nextClosestNode = _priorityQueue.Pop();

            _shortestPathTree[nextClosestNode] = _searchFrontier[nextClosestNode];

            if (nextClosestNode == end.Index)
            {
                Success = true;
                return this;
            }

            foreach (GraphEdge edge in graph.GetEdgesFrom(nextClosestNode))
            {
                float newCost = _costToNode[nextClosestNode] + edge.Cost + heuristic(graph.GetNode(edge.To), end);

                if (limitCost
                    && edge.To != end.Index
                    && edge.Cost > MaxCost)
                {
                    continue;
                }

                if (null == _searchFrontier[edge.To])
                {
                    _costToNode[edge.To] = newCost;

                    _priorityQueue.Insert(edge.To);

                    _searchFrontier[edge.To] = edge;
                }
                else if (newCost < _costToNode[edge.To]
                         && null == _shortestPathTree[edge.To])
                {
                    _costToNode[edge.To] = newCost;

                    _priorityQueue.UpdatePriority(edge.To);

                    _searchFrontier[edge.To] = edge;
                }
            }
        }

        return this;
    }
}