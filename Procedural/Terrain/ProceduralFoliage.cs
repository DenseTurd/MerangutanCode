using System.Collections.Generic;
using UnityEngine;

public class ProceduralFoliage : MonoBehaviour
{
    public List<GameObject> trees;
    [Range(0.01f, 2.5f)] public float treeDensity = 0.3f;
    [Range(0.01f, 5f)] public float treeGroupProbability = 2.5f;

    Mesh mesh;

    public void Init()
    {
        mesh = this.GetComponentOrComplain<MeshCollider>().sharedMesh;
        PlaceTrees();
    }

    void PlaceTrees()
    {
        if (!mesh) return;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            if (normals[i].y > 0.9)
            {
                if (UnityEngine.Random.Range(0f, 10f) < treeDensity)
                {
                    var tweee = Instantiate(trees.GetRandom(), transform);
                    tweee.transform.position = transform.position + mesh.vertices[i];
                    RandomiseSize(tweee);
                    RandomiseRotation(tweee);
                    TryMakeTreeGroup(tweee.transform.position);
                }
            }
        }
    }

    void RandomiseRotation(GameObject tweee)
    {
        float angle = UnityEngine.Random.Range(0, 359);
        tweee.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void RandomiseSize(GameObject tweee)
    {
        float scale = UnityEngine.Random.Range(0.5f, 2);
        tweee.transform.localScale = new Vector3(scale, scale, scale);
    }

    void TryMakeTreeGroup(Vector3 pos)
    {
        if (UnityEngine.Random.Range(0f, 10f) < treeGroupProbability)
        {
            int noOfTrees = UnityEngine.Random.Range(3, 8);
            for (int i = 0; i <= noOfTrees; i++)
            {
                var twee = Instantiate(trees.GetRandom(), transform);
                Vector2 randomCircleXY = ((Vector3)UnityEngine.Random.insideUnitCircle * 50);
                Vector3 randomCircleXZ = new Vector3(randomCircleXY.x, 0, randomCircleXY.y);
                twee.transform.position = pos + randomCircleXZ;
                RandomiseSize(twee);
                RandomiseRotation(twee);
            }
        }
    }
}
