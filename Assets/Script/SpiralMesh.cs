using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpiralMesh : MonoBehaviour
{
    public float MaxRadius = 3.0f;
    public float BackwardsSlope = 0.1f;
    public float Thickness = 0.5f;
    public float ThicknessFactor = 1.0f;
    public int Sides = 12;
    public int Rings = 4;
    public int NumberOfSprites = 1;

    void Awake()
    {

    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            Create();
        }
    }

    private float X(float radius, float angle)
    {
        return radius * Mathf.Cos(angle);
    }

    private float Y(float radius, float angle)
    {
        return radius * Mathf.Sin(angle);
    }

    [ContextMenu("Create")]
    void Create()
    {
        float angle = Mathf.PI * 0.5f;
        float angleAdvance = -(Mathf.PI * 2.0f / Sides);
        float offset = -angleAdvance * 0.5f;
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        int faces = Sides * Rings;
        int segmentSize = 0;
        int posInSegment = 0;
        int spriteIndex = 0;

        angle -= offset;

        Vector3[] vertices = new Vector3[faces * 4];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[faces * 2 * 3];

        mesh.Clear();

        for (int i = 0; i < faces * 4; i += 4)
        {
            int faceIndex = i / 4;
            float faceNormalized = (float)faceIndex / faces;
            float nextFaceNormalized = (float)(faceIndex + 1) / faces;
            float startRadius = MaxRadius * (1 - faceNormalized);
            float endRadius = MaxRadius * (1 - nextFaceNormalized);
            float startZ = BackwardsSlope * faceIndex;
            float endZ = BackwardsSlope * (faceIndex + 1);
            float startThickness = Thickness * (1 - (faceNormalized * ThicknessFactor));
            float endThickness = Thickness * (1 - (nextFaceNormalized * ThicknessFactor));
            int bl = i + 0;
            int tl = i + 1;
            int tr = i + 2;
            int br = i + 3;

            Vector3 bottomLeft = new Vector3(X(endRadius - endThickness * 0.5f, angle - offset), Y(endRadius - endThickness * 0.5f, angle - offset), endZ);
            Vector3 topLeft = new Vector3(X(endRadius + endThickness * 0.5f, angle - offset), Y(endRadius + endThickness * 0.5f, angle - offset), endZ);
            Vector3 topRight = new Vector3(X(startRadius + startThickness * 0.5f, angle + offset), Y(startRadius + startThickness * 0.5f, angle + offset), startZ);
            Vector3 bottomRight = new Vector3(X(startRadius - startThickness * 0.5f, angle + offset), Y(startRadius - startThickness * 0.5f, angle + offset), startZ);

            vertices[bl] = bottomLeft;
            vertices[tl] = topLeft;
            vertices[tr] = topRight;
            vertices[br] = bottomRight;

            normals[bl] = Vector3.back;
            normals[tl] = Vector3.back;
            normals[tr] = Vector3.back;
            normals[br] = Vector3.back;

            triangles[faceIndex * 6 + 0] = bl;
            triangles[faceIndex * 6 + 1] = tl;
            triangles[faceIndex * 6 + 2] = tr;
            triangles[faceIndex * 6 + 3] = bl;
            triangles[faceIndex * 6 + 4] = tr;
            triangles[faceIndex * 6 + 5] = br;

            if (posInSegment >= segmentSize)
            {
                posInSegment = 0;
                segmentSize = Random.Range(Sides / 4, Sides / 2);
                spriteIndex = Random.Range(0, NumberOfSprites);
            }

            float size = (float)1 / NumberOfSprites;
            float spritePos = (float)spriteIndex / NumberOfSprites + size * 0.5f;
            float min = spritePos - size * 0.5f;
            float max = spritePos + size * 0.5f;

            uv[bl] = new Vector2(min * 1.0f, 0.0f);
            uv[br] = new Vector2(max * 1.0f, 0.0f);
            uv[tl] = new Vector2(min * 1.0f, 1.0f);
            uv[tr] = new Vector2(max * 1.0f, 1.0f);

            posInSegment++;

            angle += angleAdvance;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.UploadMeshData(false);
    }

    /*
    void OnGUI()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 worldPosition = transform.TransformPoint(mesh.vertices[i]);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            Rect position = new Rect(screenPosition.x - 50, Screen.height - screenPosition.y - 20, 100, 20);
            string pos = i % 4 == 0 ? "BL" :
                         i % 4 == 1 ? "TL" :
                         i % 4 == 2 ? "TR" :
                         "BR";

            if (i % 4 == 0)
            {
                position.x -= 10.0f;
                position.y += 10.0f;
            }
            if (i % 4 == 1)
            {
                position.x -= 10.0f;
                position.y -= 10.0f;
            }
            if (i % 4 == 2)
            {
                position.x += 10.0f;
                position.y -= 10.0f;
            }
            if (i % 4 == 3)
            {
                position.x += 10.0f;
                position.y += 10.0f;
            }
            //GUI.Label(position, string.Format("({0}): {1},{2}", i, mesh.vertices[i].x.ToString("N2"), mesh.vertices[i].y.ToString("N2"), mesh.vertices[i].z.ToString("N2")));
            GUI.Label(position, pos);
        }
    }

    void OnDrawGizmos()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        for (int i = mesh.vertexCount - 1; i >= 0; i--)
        {
            Vector3 worldPosition = transform.TransformPoint(mesh.vertices[i]);

            Color color = Color.Lerp(Color.red, Color.white, (float)i / mesh.vertexCount);
            Gizmos.color = color;
            Gizmos.DrawSphere(worldPosition, 0.1f);
        }
    }
    */

}