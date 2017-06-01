using System.Collections.Generic;
using TheGoldenMule.Geo;
using UnityEngine;

public class GeoGraphRenderer : IGraphRenderer
{
    public Material Material { get; set; }
    public Transform Root { get; private set; }

    private readonly List<GameObject> _nodes = new List<GameObject>();
    private readonly List<GameObject> _edges = new List<GameObject>();

    public void Initialize(GraphContext context)
    {
        Root = new GameObject("Root").transform;

        // create geo for all nodes!
        for (var i = 0; i < context.Nodes.Count; i++)
        {
            var node = context.Nodes[i];
            var nodeGameObject = NodeGameObject(node);

            _nodes.Add(nodeGameObject);

            // create a line for each edge out
            var edgesOut = context.Graph.GetEdgesFrom(node.Node);
            foreach (var edge in edgesOut)
            {
                var edgeGameObject = EdgeGameObject(
                    context.Nodes[edge.From],
                    context.Nodes[edge.To]);
                edgeGameObject.transform.parent = nodeGameObject.transform;
            }
        }
    }

    public void Tick(float dt, GraphContext context)
    {
        if (null != Root)
        {
            Root.RotateAround(Vector3.up, Vector3.up, 10 * dt);
        }
    }

    public void Uninitialize()
    {
        
    }

    private GameObject NodeGameObject(GraphNodeContextData node)
    {
        var gameObject = new GameObject();
        gameObject.transform.position = node.Position;
        gameObject.transform.parent = Root;
        
        var mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>().material = Material;

        GeometryProvider.Build(
            mesh,
            new IcosahedronGeometryBuilder(),
            new GeometryBuilderSettings
            {
                Vertex = new GeometryBuilderVertexSettings
                {
                    Scale = node.Scale,
                    Rotation = node.Rotation
                }
            });

        return gameObject;
    }

    private GameObject EdgeGameObject(
        GraphNodeContextData start,
        GraphNodeContextData end)
    {
        var gameObject = new GameObject();



        return gameObject;
    }
}