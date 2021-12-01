using UnityEngine;

public class NevilleChase : MonoBehaviour
{
    [Range(3,60)]
    public float minSpeed = 4f;
    public float speed;
    void Update()
    {
        Vector2 dir = (Overseer.Instance.player.transform.position - transform.position).normalized;
        float distToPlayer = Vector2.Distance(Overseer.Instance.player.transform.position, transform.position);
        speed = Mathf.Max(minSpeed, distToPlayer * 2f);

        transform.position += (Vector3)dir * Time.deltaTime * speed;
    }
}
