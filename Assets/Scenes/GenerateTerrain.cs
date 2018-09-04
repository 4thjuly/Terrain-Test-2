using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour {

    static int QUADS_PER_SIDE = 50;
    static float HEIGHT_SCALE = 0.25f;
    static float DETAIL_SCALE = 0.5f;
    static float WATER_LEVEL = 0.075f;

    // Use this for initialization
    void Start ()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int numTriangles = QUADS_PER_SIDE * QUADS_PER_SIDE * 6;
        int numVertices = (QUADS_PER_SIDE + 1) * (QUADS_PER_SIDE + 1);

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];

        int index = 0;
        float uvFactor = 1.0f / QUADS_PER_SIDE;
        float scale = 1.0f / QUADS_PER_SIDE;

        for (float y = 0.0f; y < QUADS_PER_SIDE + 1; y++) {
            for (float x = 0.0f; x < QUADS_PER_SIDE + 1; x++) {
                vertices[index] = new Vector3(x * scale - 1.0f / 2.0f, 0.0f, y * scale - 1.0f / 2.0f);
                index++;
            }
        }

        index = 0;
        for (int y = 0; y < QUADS_PER_SIDE; y++) {
            for (int x = 0; x < QUADS_PER_SIDE; x++) {
                triangles[index] = (y * (QUADS_PER_SIDE + 1)) + x;
                triangles[index + 1] = ((y + 1) * (QUADS_PER_SIDE + 1)) + x;
                triangles[index + 2] = (y * (QUADS_PER_SIDE + 1)) + x + 1;

                triangles[index + 3] = ((y + 1) * (QUADS_PER_SIDE + 1)) + x;
                triangles[index + 4] = ((y + 1) * (QUADS_PER_SIDE + 1)) + x + 1;
                triangles[index + 5] = (y * (QUADS_PER_SIDE + 1)) + x + 1;
                index += 6;
            }
        }

        // Terrain adjust
        float offset = Random.Range(1.0f, 5.0f);
        for (int i = 0; i < vertices.Length; i++)  {
            vertices[i].y = Mathf.PerlinNoise((vertices[i].x + this.transform.position.x + offset) / DETAIL_SCALE, (vertices[i].z + this.transform.position.z) / DETAIL_SCALE) * HEIGHT_SCALE;
            if (vertices[i].y < WATER_LEVEL) { vertices[i].y = WATER_LEVEL - 0.01f; }
        }

        // Separate for flat look and color
        Vector3[] newVertices = new Vector3[triangles.Length];
        Color32[] colors = new Color32[triangles.Length];
        Color32 currentColor = new Color(0.0f, Random.Range(0.25f, 0.75f), 0.0f, 1.0f);
        Color32 waterColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        for (int i = 0; i < triangles.Length; i++) {
            newVertices[i] = vertices[triangles[i]];
            triangles[i] = i;
            colors[i] = currentColor; 
            if ((i + 1) % 6 == 0) {
                if (newVertices[i].y < WATER_LEVEL) { currentColor = waterColor; }
                else { currentColor = new Color(0.0f, Random.Range(0.25f, 0.75f), 0.0f, 1.0f); }
            }
        }

        //// Color
        //for (int i = 0; i < triangles.Length; )
        //{
        //    // Color32 color = new Color(Random.Range(0.0f, 1.0f),  Random.Range(0.0f, 1.0f),  Random.Range(0.0f, 1.0f),  1.0f);
        //    Color32 color = new Color(0.0f, Random.Range(0.25f, 0.75f), 0.0f, 1.0f);
        //    colors[i++] = color;
        //    colors[i++] = color;
        //    colors[i++] = color;
        //    colors[i++] = color;
        //    colors[i++] = color;
        //    colors[i++] = color;
        //}

        mesh.vertices = newVertices;
        mesh.triangles = triangles;
        mesh.colors32 = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
	}

    float MOVE_SPEED = 0.1f;
    float offsetX;

    void Update()
    {
    //    Mesh mesh = GetComponent<MeshFilter>().mesh;
    //    Vector3[] vertices = mesh.vertices;

    //    this.offsetX += MOVE_SPEED * Time.deltaTime;

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        vertices[i].y = Mathf.PerlinNoise((vertices[i].x + this.transform.position.x + this.offsetX) / DETAIL_SCALE, (vertices[i].z + this.transform.position.z) / DETAIL_SCALE) * HEIGHT_SCALE;
    //    }

    //    mesh.vertices = vertices;
    //    mesh.RecalculateNormals();
    //    mesh.RecalculateBounds();
    }
}
