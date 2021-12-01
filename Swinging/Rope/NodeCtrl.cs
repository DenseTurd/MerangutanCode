using UnityEngine;

public class NodeCtrl : MonoBehaviour
{
    public Transform mesh;
    public Transform nextNode;

    void Update()
    {
        if (mesh)
        {
            if (nextNode)
            {
                LookAndStretch(nextNode);
            }
            else
            {
                LookAndStretch(Overseer.Instance.player.transform);
            }
        }
    }

    void LookAndStretch(Transform trans)
    {
        mesh.LookAt(trans);
        mesh.transform.localScale = new Vector3(mesh.localScale.x, mesh.localScale.y, Vector2.Distance(transform.position, trans.position));
    }
}
