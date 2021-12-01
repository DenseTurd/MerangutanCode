using UnityEngine;
using System.Collections.Generic;

public class DeadlyFogGen : MonoBehaviour
{
    public GameObject deadlyFog;
    Mesh mesh;
    public List<GameObject> fogs;
    Queue<GameObject> fogsQueue;

    ProceduralTerrain proceduralTerrain;
    public int unitLength;
    public int xSize;

    public void Init()
    {
        mesh = this.GetComponentOrComplain<MeshCollider>().sharedMesh;
        fogsQueue = new Queue<GameObject>(fogs);
        proceduralTerrain = this.GetComponentOrComplain<ProceduralTerrain>();
        unitLength = proceduralTerrain.unitLength;
        xSize = proceduralTerrain.xSize;
        SortFog();
    }

    void SortFog()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (transform.position.z + mesh.vertices[i].z > -unitLength * 0.5f && transform.position.z + mesh.vertices[i].z < unitLength * 0.5f)
            {
                PutFogsOnRow(i);
                return;
            }
        }
    }

    void PutFogsOnRow(int startingVerticie)
    {
        Debug.Log($"Starting fog placement on verticie {startingVerticie}");
        for (int i = startingVerticie; i < startingVerticie + xSize; i++)
        {
            var fog = fogsQueue.Dequeue();
            fog.transform.position = new Vector3(transform.position.x + mesh.vertices[i].x, transform.position.y + mesh.vertices[i].y, 0);
            fog.transform.localScale = new Vector3(unitLength, 1, 1);
        }
    }
}
