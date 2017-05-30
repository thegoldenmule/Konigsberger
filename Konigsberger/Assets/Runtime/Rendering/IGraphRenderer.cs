using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphNodeContextData
{
    public string NodeGuid;

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}

[Serializable]
public class GraphContext
{
    public List<GraphNodeContextData> Nodes = new List<GraphNodeContextData>();
}

public interface IGraphRenderer
{
    void Initialize(Graph graph, GraphContext context);
    void Tick(float dt, GraphContext context);
    void Uninitialize();
}