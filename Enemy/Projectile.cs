using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 destination;
    Vector3 dir;
    float lifeTime = 1.6f;
    float force = 50f;
    Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponentOrComplain<Rigidbody2D>();

        Vector2 relativeDir = (destination - transform.position);
        dir = new Vector2(relativeDir.x, (Mathf.Abs(relativeDir.x) + relativeDir.y));
        rb.AddForce(dir * force * Random.Range(1f, 1.3f));
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
