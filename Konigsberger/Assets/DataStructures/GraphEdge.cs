using System;

[Serializable]
public class GraphEdge
{
    public int From;
    public int To;
    public float Cost;

    public GraphEdge(int from, int to)
    {
        From = from;
        To = to;
    }

    public override string ToString()
    {
        return string.Format("[GraphEdge From={0}, To={1}]",
            From,
            To);
    }
}