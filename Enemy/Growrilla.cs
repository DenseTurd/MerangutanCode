using UnityEngine;

public class Growrilla : Enemy
{
    const float startingScale = 1;
    float scale = 1;

    Vector2 startingPos;
    Quaternion startingRot;
    Rigidbody2D rb2D;

    public ParticleSystem poofParticles;
    const float popTime = 1.6f;
    float popTimer = 1.6f;
    bool popping;

    public override void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        rb2D = this.GetComponentOrComplain<Rigidbody2D>();
        base.Start();
    }

    void Update()
    {
        if (active)
        {
            scale += Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        if (popping)
        {
            popTimer -= Time.deltaTime;
        }

        if (popTimer <= 0)
        {
            Pop();
        }

        if (scale > 10)
        {
            Pop();
        }
    }

    public void Pop()
    {
        poofParticles.transform.parent = null;
        poofParticles.Play();

        Overseer.Instance.audioManager.enemyAudio.Death(this);

        transform.position = new Vector3(spawnPos.x, spawnPos.y - 10000, spawnPos.z);

        Overseer.Instance.audioManager.adaptiveMusicManager.EnemyDistanceCheck();

        popping = false;
        popTimer = popTime;
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            popping = true;
        }
    }

    public override void ResetMe()
    {
        scale = startingScale;
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = startingPos;
        transform.rotation = startingRot;
        rb2D.velocity = Vector3.zero;

        poofParticles.transform.parent = this.transform;
        poofParticles.transform.localPosition = Vector3.zero;
    }
}
