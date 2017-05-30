﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Example1Bootstrap : MonoBehaviour
{
    public class GraphGenerationParameters
    {
        public int NumNodes = 14;
        public int NumEdges = 24;
    }

    public bool GenerateGraph;

    private void Update()
    {
        if (GenerateGraph)
        {
            GenerateGraph = false;


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

        var data = new GraphData
        {
            Nodes = Enumerable
                .Range(0, parameters.NumNodes)
                .Select(i => new GraphNode
                {
                    Guid = Guid.NewGuid().ToString(),
                    Label = _alphabet[i % _alphabet.Length],
                    Index = i
                })
                .ToArray()
        };

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

            edges.Add(new GraphEdge(from, to));
        }

        data.Edges = edges.ToArray();

        return new Graph(data);
    }
}