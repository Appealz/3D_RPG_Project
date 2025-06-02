using UnityEngine;

public static class CircleMeshGenerator
{
    public static Mesh CreateCircleMesh(float radius = 1f, int segments = 64)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float angleStep = 360f / segments;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;            
            vertices[i] = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 2 > segments) ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
