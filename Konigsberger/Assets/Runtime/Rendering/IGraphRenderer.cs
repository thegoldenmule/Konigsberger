using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphNodeRenderData
{
    public string NodeGuid;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}

[Serializable]
public class GraphRenderData
{
    public List<GraphNodeRenderData> RenderData = new List<GraphNodeRenderData>();
}

public interface IGraphRenderer
{
    void Initialize(Graph graph, GraphRenderData renderData);
    void Tick(float dt, Bounds bounds);
    void Uninitialize();
}