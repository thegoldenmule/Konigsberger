using UnityEngine;

public class GraphNodeRenderData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}

public class GraphRenderData
{
    public Graph Graph { get; private set; }
}

public interface IGraphRenderer
{
    void Initialize(Graph graph);
    void Layout();
    void Draw(Bounds bounds);
    void Uninitialize(Graph graph);
}