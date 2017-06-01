using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Example1Bootstrap : MonoBehaviour
{
    [Serializable]
    public class GraphGenerationParameters
    {
        public enum LayoutType
        {
            Circle
        }

        public enum RendererType
        {
            Geo
        }

        public int NumNodes = 100;
        public int NumEdges = 24;

        public LayoutType Layout;
        public RendererType Renderer;
    }

    public bool GenerateGraph;
    public GraphGenerationParameters Parameters;
    public Material Material;
    public float Size = 10;

    private GraphContext _context;
    private IGraphLayoutEngine _layout;
    private IGraphRenderer _renderer;

    private void Update()
    {
        if (GenerateGraph)
        {
            GenerateGraph = false;

            if (null != _renderer)
            {
                _renderer.Uninitialize();
            }

            if (null != _layout)
            {
                _layout.Uninitialize();
            }

            var graph = Generate(Parameters);
            _context = new GraphContext(graph);

            var sphereLayout = new SphereLayoutEngine();
            sphereLayout.Size = Size;
            _layout = sphereLayout;

            var geoRenderer = new GeoGraphRenderer();
            geoRenderer.Material = Material;
            _renderer = geoRenderer;
            
            _layout.Initialize(_context);
            _renderer.Initialize(_context);
        }

        if (null != _layout)
        {
            _layout.Tick(Time.deltaTime, _context);
        }

        if (null != _renderer)
        {
            _renderer.Tick(Time.deltaTime, _context);
        }
    }

    private static readonly string[] _alphabet = {
        "a", "b", "c", "d", "e", "f",
        "g", "h", "i", "j", "k" ,"l",
        "m", "n", "o", "p", "q", "r",
        "s", "t", "u", "v", "w", "x",
        "y", "z"
    };

    private static Graph Generate(GraphGenerationParameters parameters)
    {
        // not enough nodes
        if (parameters.NumNodes < 2)
        {
            return null;
        }

        // too many edges
        if (parameters.NumEdges > (parameters.NumNodes * (parameters.NumNodes - 1) / 2))
        {
            return null;
        }

        var graph = new Graph();

        for (var i = 0; i < parameters.NumNodes; i++)
        {
            graph.AddNode(new GraphNode
            {
                Label = _alphabet[i % _alphabet.Length]
            });
        }
        
        var edges = new List<GraphEdge>();
        while (edges.Count < parameters.NumEdges)
        {
            var from = UnityEngine.Random.Range(0, parameters.NumNodes);
            var to = UnityEngine.Random.Range(0, parameters.NumNodes);

            if (from == to)
            {
                continue;
            }

            var found = false;
            for (var j = 0; j < edges.Count; j++)
            {
                if (edges[j].To == to && edges[j].From == from)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                continue;
            }

            var edge = new GraphEdge(from, to);
            edges.Add(edge);

            graph.AddEdge(edge);
        }

        return graph;
    }
}