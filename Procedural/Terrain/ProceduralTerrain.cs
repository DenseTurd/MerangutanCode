using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
    public Mesh mesh;
    public MeshCollider collider;
    Vector3[] verticies;
    int[] triangles;
    Vector2[] uvs;

    [Range(1, 50)] public int unitLength = 3;

    public int xSize = 200;
    public int zSize = 100;

    public float lilBumpsMultiplier = 0.2f; 
    public int smallHillsMultiplier = 10;
    public int medHillsMultiplier = 80;
    public int largeHillsMultiplier = 300;

    public void Init()
    {
        if (mesh)
        {
            mesh.Clear();
        }
        mesh = new Mesh();
        this.GetComponentOrComplain<MeshFilter>().mesh = mesh;
        collider = this.GetComponentOrComplain<MeshCollider>();
        MakeShape();
        UpdateMesh();
        collider.sharedMesh = mesh;
    }    

    void MakeShape()
    {
        verticies = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                float worldX = (transform.position.x + (x * unitLength));
                float worldZ = (transform.position.z + (z * unitLength));
                verticies[i] = new Vector3(worldX + RandomOffset(), GetNoise(x, z), worldZ + RandomOffset());
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[verticies.Length];
        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
    }

    private float RandomOffset()
    {
        return UnityEngine.Random.Range(-(unitLength * 0.4f), unitLength * 0.4f);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    float GetNoise(float x, float z)
    {
        float y = transform.position.y;
        y += Mathf.PerlinNoise(x * unitLength * 0.4f, z  * unitLength* 0.4f) * 0.2f;
        
        y += Mathf.PerlinNoise(x * unitLength * 0.05f, z * unitLength * 0.05f) * smallHillsMultiplier;

        float med = Mathf.PerlinNoise(x * unitLength * 0.01f, z * unitLength * 0.01f);
        med *= med;
        y += med * medHillsMultiplier;

        y += Mathf.PerlinNoise(x * unitLength * 0.0015f, z * unitLength * 0.0015f) * largeHillsMultiplier;

        return y;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("Do terrain again");
    //        Init();
    //    }            
    //}
}
