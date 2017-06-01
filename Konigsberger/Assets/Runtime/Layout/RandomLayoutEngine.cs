using UnityEngine;

public class RandomLayoutEngine : IGraphLayoutEngine
{
    public void Initialize(GraphContext context)
    {
        // random in bounds
        var bounds = new Bounds(Vector3.zero, 10 * Vector3.one);
        foreach (var node in context.Nodes)
        {
            node.Position = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
            node.Rotation = Random.rotation;
        }
    }

    public void Tick(float dt, GraphContext context)
    { 
        
    }

    public void Uninitialize()
    {
        
    }
}