using UnityEngine;

public class Neanderthrow : Enemy, ICrushable
{
    public Projectile projectile;
    const float timeBetweenAttacks = 0.3f;
    float attackTmer = 0.3f;
    public int ammo = 3;
    int clip;
    const float reloadTime = 1.8f;
    float reloadTimer = 2.2f;
    public ParticleSystem poofParticles;

    public override void Start()
    {
        clip = ammo;
        base.Start();
    }

    void Attack()
    {
        Projectile thrown = Instantiate(projectile);
        thrown.transform.position = new Vector2(transform.position.x, transform.position.y +1);
        thrown.destination = Overseer.Instance.player.transform.position;
        clip--;

        Overseer.Instance.audioManager.enemyAudio.Attack(this);
    }

    public void GetCrushed()
    {
        poofParticles.gameObject.transform.parent = null;
        poofParticles.Play();

        transform.position = new Vector3(spawnPos.x, spawnPos.y - 10000, spawnPos.z);
        Overseer.Instance.audioManager.adaptiveMusicManager.EnemyDistanceCheck();
        Overseer.Instance.audioManager.enemyAudio.Death(this);
        active = false;
    }

    void Update()
    {
        if (active)
        {
            if (clip > 0)
            {
                attackTmer -= Time.deltaTime;
                if (attackTmer <= 0)
                {
                    attackTmer = timeBetweenAttacks * Random.Range(0.3f, 1.2f);
                    Attack();
                }
            }
            else
            {
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0)
                {
                    clip = ammo;
                    reloadTimer = reloadTime;
                }
            }
        }
    }

    public override void ResetMe()
    {
        clip = ammo;
        transform.position = spawnPos;
        poofParticles.transform.parent = this.transform;
        poofParticles.transform.localPosition = Vector3.zero;
    }
}
