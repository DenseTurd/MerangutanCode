using UnityEngine;

public class Rope : MonoBehaviour
{
    public Vector2 hookPos;
    public float speed = 20;
    public float nodeDist = 0.25f;

    public GameObject nodePrefab;
    public GameObject player;
    public GameObject prevNode;

    bool done;

    private void Start()
    {
        prevNode = gameObject;
    }

    void Update()
    {
        if((Vector2)transform.position != hookPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, hookPos, speed * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, prevNode.transform.position) > nodeDist)
            {
                CreateNode();
            }
        }
        else if (!done)
        {
            done = true;
            prevNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            prevNode.GetComponent<SpringJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();

        }

    }

    void CreateNode()
    {
        Vector2 dir = (player.transform.position - prevNode.transform.position).normalized;
        Vector2 pos = dir * nodeDist;
        pos += (Vector2)prevNode.transform.position;

        GameObject node = Instantiate(nodePrefab);
        node.transform.position = pos;
        node.transform.parent = transform;

        prevNode.GetComponent<HingeJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();
        prevNode.GetComponent<SpringJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();

        prevNode = node;
    }
}
