using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class GraphData
{
    public GraphNode[] Nodes;
    public GraphEdge[] Edges;
}

public class Graph
{
    private readonly List<GraphNode> _nodes = new List<GraphNode>();
    private readonly List<List<GraphEdge>> _edgesFromNode = new List<List<GraphEdge>>();
    private readonly List<List<GraphEdge>> _edgesToNode = new List<List<GraphEdge>>();

    public Graph()
    {
        
    }

    public Graph(GraphData data)
    {
        for (int i = 0, len = data.Nodes.Length; i < len; i++)
        {
            AddNode(data.Nodes[i]);
        }

        for (int i = 0, len = data.Edges.Length; i < len; i++)
        {
            AddEdge(data.Edges[i]);
        }
    }

    public int NumNodes
    {
        get
        {
            return _nodes.Count;
        }
    }

    public GraphData ToData()
    {
        var from = _edgesFromNode.SelectMany(nodes => nodes);
        var to = _edgesToNode.SelectMany(nodes => nodes);

        return new GraphData
        {
            Nodes = _nodes.ToArray(),
            Edges = to.Concat(from).Distinct().ToArray()
        };
    }

    public ReadOnlyCollection<GraphNode> GetNodes()
    {
        return _nodes.AsReadOnly();
    }
    
    public IEnumerable<T> GetNodes<T>() where T : GraphNode
    {
        return _nodes.OfType<T>();
    }

    public IEnumerable<GraphEdge> GetEdgesFrom(GraphNode node)
    {
        if (null == node)
        {
            return Enumerable.Empty<GraphEdge>();
        }

        return GetEdgesFrom(node.Index);
    }

    public IEnumerable<GraphEdge> GetEdgesFrom(int index)
    {
        if (index >= 0 && index < _edgesFromNode.Count)
        {
            return _edgesFromNode[index];
        }

        return Enumerable.Empty<GraphEdge>();
    }

    public IEnumerable<T> GetEdgesFrom<T>(GraphNode node) where T : GraphEdge
    {
        if (null == node)
        {
            return Enumerable.Empty<T>();
        }

        return GetEdgesFrom<T>(node.Index);
    }

    public IEnumerable<T> GetEdgesFrom<T>(int index) where T : GraphEdge
    {
        if (index < 0 || index >= _edgesFromNode.Count)
        {
            return Enumerable.Empty<T>();
        }

        return _edgesFromNode[index].OfType<T>();
    }

    public IEnumerable<GraphEdge> GetEdgesTo(GraphNode node)
    {
        if (null == node)
        {
            return Enumerable.Empty<GraphEdge>();
        }

        return GetEdgesTo(node.Index);
    }

    public IEnumerable<GraphEdge> GetEdgesTo(int index)
    {
        if (index >= 0 && index < _edgesToNode.Count)
        {
            return _edgesToNode[index];
        }

        return Enumerable.Empty<GraphEdge>();
    }

    public IEnumerable<T> GetEdgesTo<T>(GraphNode node) where T : GraphEdge
    {
        if (null == node)
        {
            return Enumerable.Empty<T>();
        }

        return GetEdgesTo<T>(node.Index);
    }

    public IEnumerable<T> GetEdgesTo<T>(int index)
    {
        if (index < 0 || index >= _edgesToNode.Count)
        {
            return Enumerable.Empty<T>();
        }

        return _edgesToNode[index].OfType<T>();
    }

    public T GetEdge<T>(int from, int to) where T : GraphEdge
    {
        if (from < 0 || from >= _edgesFromNode.Count)
        {
            return null;
        }

        var edges = _edgesFromNode[from];
        for (int i = 0, len = edges.Count; i < len; i++)
        {
            if (edges[i].To == to)
            {
                return edges[i] as T;
            }
        }

        return null;
    }

    public T GetEdge<T>(GraphNode from, GraphNode to) where T : GraphEdge
    {
        if (null == from || null == to)
        {
            return null;
        }

        return GetEdge<T>(from.Index, to.Index);
    }

    public GraphNode GetNode(int index)
    {
        return index >= 0 && index < _nodes.Count ? _nodes[index] : null;
    }

    public T GetNode<T>(int index) where T : GraphNode
    {
        return GetNode(index) as T;
    }

    public int AddNode(GraphNode node)
    {
        if (-1 == node.Index)
        {
            node.Index = _nodes.Count;
        }

        if (string.IsNullOrEmpty(node.Guid))
        {
            node.Guid = Guid.NewGuid().ToString();
        }

        _nodes.Add(node);
        _edgesFromNode.Add(new List<GraphEdge>());
        _edgesToNode.Add(new List<GraphEdge>());

        return node.Index;
    }

    public void RemoveNode(GraphNode node)
    {
        if (-1 == node.Index)
        {
            return;
        }

        _nodes[node.Index] = null;
        _edgesFromNode[node.Index] = null;
        _edgesToNode[node.Index] = null;

        node.Index = -1;
    }

    public void AddEdge(GraphEdge edge)
    {
        _edgesFromNode[edge.From].Add(edge);
        _edgesToNode[edge.To].Add(edge);
    }

    public void RemoveEdge(GraphEdge edge)
    {
        _edgesFromNode[edge.From].Remove(edge);
        _edgesToNode[edge.To].Remove(edge);
    }
}