﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphNodeContextData
{
    public readonly GraphNode Node;

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public GraphNodeContextData(GraphNode node)
    {
        Node = node;
    }
}

[Serializable]
public class GraphContext
{
    /// <summary>
    /// The underlying graph.
    /// </summary>
    public readonly Graph Graph;

    /// <summary>
    /// List of nodes.
    /// </summary>
    public List<GraphNodeContextData> Nodes = new List<GraphNodeContextData>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="graph"></param>
    public GraphContext(Graph graph)
    {
        Graph = graph;

        var nodes = Graph.GetNodes();
        for (int i = 0, len = nodes.Count; i < len; i++)
        {
            Nodes.Add(new GraphNodeContextData(nodes[i]));
        }
    }
}

public interface IGraphRenderer
{
    void Initialize(GraphContext context);
    void Tick(float dt, GraphContext context);
    void Uninitialize();
}