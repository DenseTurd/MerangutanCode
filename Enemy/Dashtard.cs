using System;
using UnityEngine;

public class Dashtard : Enemy, ICrushable
{
    Vector2 startingPos;
    Quaternion startingRot;
    Rigidbody2D rb2D;

    public ParticleSystem poofParticles;

    int dir;
    readonly Vector2 velo = new Vector2(1, 0);
    public float speed = 10;

    public Transform wallCheckAnchor;
    public Transform wallCheck;
    float wallCheckRadius = 0.15f;

    bool stunned;
    const float stunTime = 1f;
    float stunTimer;
    public override void Start()
    {
        wallColliders = new Collider2D[64];
        startingPos = transform.position;
        startingRot = transform.rotation;
        rb2D = this.GetComponentOrComplain<Rigidbody2D>();
        base.Start();
    }

    void Update()
    {
        if (active)
        {
            DashTowardPlayer();
        }

        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                stunned = false;
            }
        }
    }

    void DashTowardPlayer()
    {
        if (stunned) return;
        dir = Overseer.Instance.player.transform.position.x < transform.position.x ? -1 : 1;
        rb2D.velocity = new Vector2(velo.x * dir * speed, rb2D.velocity.y);
        WallChecks();
    }

    public override void ResetMe()
    {
        transform.position = startingPos;
        transform.rotation = startingRot;
        rb2D.velocity = Vector3.zero;

        poofParticles.transform.parent = this.transform;
        poofParticles.transform.localPosition = Vector3.zero;
    }

    public void GetCrushed()
    {
        poofParticles.transform.parent = null;
        poofParticles.Play();
        Overseer.Instance.audioManager.enemyAudio.Death(this);

        transform.position = new Vector3(spawnPos.x, spawnPos.y - 10000, spawnPos.z);
        active = false;
        Overseer.Instance.audioManager.adaptiveMusicManager.EnemyDistanceCheck();
    }

    readonly int deadlyLayer = 12;
    readonly int dangerousLayer = 13;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == deadlyLayer)
        {
            GetCrushed();
            return;
        }

        if (collision.gameObject.layer == dangerousLayer)
        {
            GetCrushed();
            return;
        }
    }

    Collider2D[] wallColliders;
    int groundlayerMask = 1 << 6;
    void WallChecks()
    {
        wallCheck.transform.position = wallCheckAnchor.transform.position + ((Vector3)velo * dir * 0.5f);
        int noOfWallColliders = Physics2D.OverlapCircleNonAlloc(wallCheck.position, wallCheckRadius, wallColliders, groundlayerMask);
        for (int i = 0; i < noOfWallColliders; i++)
        {
            if (wallColliders[i].gameObject != gameObject)
            {
                rb2D.velocity = velo * -dir * speed * 0.25f;
                stunned = true;
                stunTimer = stunTime;
                poofParticles.Play();
                Overseer.Instance.audioManager.enemyAudio.Attack(this);
            }
        }
    }
}
