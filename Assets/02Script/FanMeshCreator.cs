using UnityEngine;

public static class FanMeshGenerator
{
    public static Mesh CreateFanMesh(float radius = 3f, float angleDegree = 90f, int segments = 30)
    {
        Mesh mesh = new Mesh();

        // 꼭지점 = 중심 + 각도별 점들
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 중심

        float halfAngle = angleDegree / 2f;
        float angleStep = angleDegree / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (-halfAngle + angleStep * i);
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;
            vertices[i + 1] = new Vector3(x, 0f, z);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
