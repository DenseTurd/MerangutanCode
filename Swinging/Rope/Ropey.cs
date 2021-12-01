using UnityEngine;

public class Ropey : MonoBehaviour
{
    public Vector2 hookPos;
    float throwSpeed = 80;

    public GameObject nodePrefab;
    public Swinger swinger;
    public GameObject prevNode;

    bool done;

    int layerMask;

    private void Start()
    {
        prevNode = gameObject;
        layerMask = 1 << 6;
    }

    void Update()
    {
        if (Vector2.Distance((Vector2)transform.position, hookPos) > 1)
        {
            transform.position += ((Vector3)hookPos - transform.position).normalized * Time.deltaTime * throwSpeed;
        }
        else if (!done)
        {
            PositionNodes();
            done = true;
            prevNode.GetComponent<HingeJoint2D>().connectedBody = swinger.GetComponent<Rigidbody2D>();

            transform.position = hookPos;

            Overseer.Instance.audioManager.playerAudio.SwingConnect(transform.position);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.25f, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Debug.Log("Swing hook hit something");
                swinger.StopSwing();

                Overseer.Instance.audioManager.playerAudio.SwingFailToConnect();
            }
        }
    }

    void PositionNodes()
    {
        Vector2 dir = (swinger.transform.position - transform.position).normalized;
        float dist = Vector2.Distance(swinger.transform.position, transform.position);

        Vector2 pos;

        for (int i = 0; i < 16; i++)
        {
            pos = (dir * ((dist / 16) * i)) + (Vector2)transform.position;
            PlaceNode(pos);
        }

        PlaceNode(swinger.transform.position);
    }

    void PlaceNode(Vector2 pos)
    {
        GameObject node = Instantiate(nodePrefab);
        node.transform.position = pos;
        node.transform.parent = transform;

        prevNode.GetComponent<NodeCtrl>().nextNode = node.transform;

        prevNode.GetComponent<HingeJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();
        //prevNode.GetComponent<SpringJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();

        prevNode = node;
    }
}
