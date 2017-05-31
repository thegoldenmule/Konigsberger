using System.Collections.Generic;
using TheGoldenMule.Geo;
using UnityEngine;

public class GeoGraphRenderer : IGraphRenderer
{
    private readonly List<GameObject> _nodes = new List<GameObject>();

    public void Initialize(GraphContext context)
    {
        // create geo for all nodes!
        for (var i = 0; i < context.Nodes.Count; i++)
        {
            var node = context.Nodes[i];
            var gameObject = NodeGameObject(node);

            _nodes.Add(gameObject);
        }

        // create a line for each edge!

    }

    public void Tick(float dt, GraphContext context)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    private GameObject NodeGameObject(GraphNodeContextData node)
    {
        var gameObject = new GameObject();
        gameObject.transform.position = node.Position;
        
        var mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>();

        GeometryProvider.Build(
            mesh,
            new StandardGeometryBuilder(),
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
}