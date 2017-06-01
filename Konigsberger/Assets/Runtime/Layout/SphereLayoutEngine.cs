using UnityEngine;

public class SphereLayoutEngine : IGraphLayoutEngine
{
    public float Size { get; set; }

    public void Initialize(GraphContext context)
    {
        // distribute nodes on a sphere
        var nodes = context.Nodes;
        var nodeCount = nodes.Count;

        var rnd = Random.value * nodeCount;
        var offset = 2f / nodeCount;
        var increment = Mathf.PI * (3 - Mathf.Sqrt(5));
        for (var i = 0; i < nodeCount; i++)
        {
            var node = nodes[i];

            var y = ((i * offset) - 1) + (offset / 2);
            var r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));

            var phi = ((i + rnd) % nodeCount) * increment;

            var x = Mathf.Cos(phi) * r;
            var z = Mathf.Sin(phi) * r;

            node.Position = Size * new Vector3(x, y, z);
        }
    }

    public void Tick(float dt, GraphContext context)
    {
        //
    }

    public void Uninitialize()
    {
        //
    }
}