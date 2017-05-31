using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class LinePathRenderer
{
    private Mesh _mesh;

    private enum QuadVert
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    };

    public struct WaypointData
    {
        public Vector3 Vertex;
        public Color Color;
    }

    public float Width
    {
        get;
        set;
    }

    public GameObject View
    {
        get;
        private set;
    }

    public LinePathRenderer(Transform parent)
    {
        View = new GameObject("LinePathRenderer");
        View.AddComponent<MeshRenderer>();
        View.AddComponent<MeshFilter>();

        View.transform.parent = parent;
    }

    public void Destroy()
    {
        _mesh = null;

        Object.Destroy(View);
        View = null;
    }

    public virtual void SetWaypoints(List<Vector3> waypoints)
    {
        SetWaypoints(waypoints.Select
        (waypoint => new WaypointData
        {
            Vertex = waypoint
        }).ToList());
    }

    public virtual void SetWaypoints(List<WaypointData> waypoints)
    {
        // invalid
        if (waypoints == null || waypoints.Count < 2)
        {
            View.SetActive(false);
            return;
        }

        View.SetActive(true);

        int numberOfQuads = waypoints.Count - 1;

        _mesh = new Mesh();

        float totalDistance = GetPathDistance(waypoints);
        float currentDistance = 0f;

        var uvs = new Vector2[numberOfQuads * 4];
        var vertices = new Vector3[numberOfQuads * 4];
        var colors = new Color[numberOfQuads * 4];
        var triangles = new int[numberOfQuads * 6];

        for (int i = 0; i < numberOfQuads; i++)
        {
            WaypointData startWaypoint = waypoints[i];
            WaypointData endWaypoint = waypoints[i + 1];

            Vector3 currentForward = (endWaypoint.Vertex - startWaypoint.Vertex).normalized;
            Vector3 pastForward = i > 0
                ? (waypoints[i].Vertex - waypoints[i - 1].Vertex).normalized
                : currentForward;
            Vector3 futureForward = i < numberOfQuads - 1
                ? (waypoints[i + 2].Vertex - waypoints[i + 1].Vertex).normalized
                : currentForward;

            bool justTurned = Vector3.Dot(currentForward, pastForward) < Mathf.Epsilon;
            bool aboutToTurn = Vector3.Dot(currentForward, futureForward) < Mathf.Epsilon;

            float normalizedStart = currentDistance / totalDistance;

            currentDistance += Vector3.Distance(startWaypoint.Vertex, endWaypoint.Vertex);
            float normalizedEnd = currentDistance / totalDistance;

            AddQuad(
                i,
                normalizedStart,
                normalizedEnd,
                Width,
                justTurned,
                aboutToTurn || i == numberOfQuads - 1,
                startWaypoint,
                endWaypoint,
                ref uvs,
                ref vertices,
                ref colors,
                ref triangles);
        }

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
        ;

        View.GetComponent<MeshFilter>().mesh = _mesh;
    }

    private static void AddQuad(
        int pathIndex,
        float normalizedStart,
        float normalizedEnd,
        float width,
        bool shrinkStart,
        bool expandEnd,
        WaypointData startWaypoint,
        WaypointData endWaypoint,
        ref Vector2[] uvs,
        ref Vector3[] vertices,
        ref Color[] colors,
        ref int[] triangles)
    {
        float halfWidth = width / 2f;

        int vertIndex = (pathIndex * 4);
        int triangleIndex = (pathIndex * 6);

        Vector3 start = startWaypoint.Vertex;
        Vector3 end = endWaypoint.Vertex;

        Vector3 forward = (end - start).normalized;
        Vector3 right = GetUnitHorizontalPerpendicular(start, end);

        if (shrinkStart)
        {
            start += halfWidth * forward;
        }

        if (expandEnd)
        {
            end += halfWidth * forward;
        }

        vertices[vertIndex + (int)QuadVert.TopLeft] = start + right * halfWidth;
        vertices[vertIndex + (int)QuadVert.BottomLeft] = start - right * halfWidth;
        vertices[vertIndex + ((int)QuadVert.TopRight)] = end + right * halfWidth;
        vertices[vertIndex + ((int)QuadVert.BottomRight)] = end - right * halfWidth;

        uvs[vertIndex + (int)QuadVert.TopLeft] = new Vector2(normalizedStart, 1f);
        uvs[vertIndex + (int)QuadVert.BottomLeft] = new Vector2(normalizedStart, 0f);
        uvs[vertIndex + (int)QuadVert.TopRight] = new Vector2(normalizedEnd, 1f);
        uvs[vertIndex + (int)QuadVert.BottomRight] = new Vector2(normalizedEnd, 0f);

        colors[vertIndex + (int)QuadVert.TopLeft] = startWaypoint.Color;
        colors[vertIndex + (int)QuadVert.BottomLeft] = startWaypoint.Color;
        colors[vertIndex + (int)QuadVert.TopRight] = endWaypoint.Color;
        colors[vertIndex + (int)QuadVert.BottomRight] = endWaypoint.Color;

        triangles[triangleIndex++] = vertIndex + (int)QuadVert.BottomLeft;
        triangles[triangleIndex++] = vertIndex + (int)QuadVert.TopLeft;
        triangles[triangleIndex++] = vertIndex + (int)QuadVert.TopRight;
        triangles[triangleIndex++] = vertIndex + (int)QuadVert.BottomLeft;
        triangles[triangleIndex++] = vertIndex + (int)QuadVert.TopRight;
        triangles[triangleIndex] = vertIndex + (int)QuadVert.BottomRight;
    }

    private static Vector3 GetUnitHorizontalPerpendicular(Vector3 startPosition, Vector3 endPosition)
    {
        // handle degenerate cases
        if (Mathf.Abs(endPosition.x - startPosition.x) < Mathf.Epsilon)
        {
            if (endPosition.z < startPosition.z)
            {
                return Vector3.right;
            }

            return -Vector3.right;
        }

        // get the direction we're going
        Vector3 direction = (endPosition - startPosition).normalized;

        return new Vector3(-direction.z, 0f, direction.x);
    }

    private static float GetPathDistance(List<WaypointData> waypoints)
    {
        float distance = 0;
        for (int i = 0, len = waypoints.Count - 1; i < len; i++)
        {
            distance += Vector3.Distance(waypoints[i].Vertex, waypoints[i + 1].Vertex);
        }
        return distance;
    }
}