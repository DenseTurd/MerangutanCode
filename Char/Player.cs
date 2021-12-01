using UnityEngine;

public class Player : MonoBehaviour, IHaveHealth
{
    public Health health;

    bool invulnerable;
    const float invulnerableTime = 0.8f;
    float invulnerableTimer;
    const float flashTime = 0.1f;
    float flashTimer;
    public GameObject mesh;
    CharController charController;

    void Start()
    {
        health = new Health(2);
        charController = this.GetComponentOrComplain<CharController>();
    }

    public void TakeDamage()
    {
        if (!invulnerable)
        {
            health.TakeDamage();
            BecomeInvulnerable();
            if (health.Ded()) Overseer.Instance.fails.Respawn();
            CamShake.Shake(0.8f, 11);

            Overseer.Instance.audioManager.adaptiveMusicManager.highHealthLayer.vol = 0;
            // audio call
        }
    }

    private void BecomeInvulnerable()
    {
        invulnerable = true;
        invulnerableTimer = invulnerableTime;
    }

    public void Heal(int healing)
    {
        health.Heal(healing);
        if (health.hp == health.maxHp)
        {
            Overseer.Instance.audioManager.adaptiveMusicManager.highHealthLayer.vol = 1;
        }
    }

    void Update()
    {
        if (invulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                mesh.SetActive(!mesh.activeSelf);
                flashTimer = flashTime;
            }
            if (invulnerableTimer <= 0)
            {
                invulnerable = false;
                mesh.SetActive(true);
            }
        }
    }

    ICrushable crushable;
    Volcano volcano;
    StayActiveVolcano stayActiveVolcano;
    LevelEnd levelEnd;
    readonly int deadlyLayer = 12;
    readonly int dangerousLayer = 13;
    readonly int collectableLayer = 14;
    readonly int volcanoLayer = 15;
    readonly int levelEndLayer = 16;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == deadlyLayer)
        {
            Overseer.Instance.fails.Respawn();
            return;
        }

        if (collision.CompareTag(Strs.crushable))
        {
            crushable = InterfaceUtility.GetInterface<ICrushable>(collision.gameObject);
            if (crushable != null)
            {
                if (charController.rb2D.velocity.y < 0)
                {
                    charController.EnemyBounce();
                    crushable.GetCrushed();
                    CamShake.Shake(0.4f, 4f);
                    crushable = null;
                    return;
                }
            }
        }

        if (collision.gameObject.layer == dangerousLayer)
        {
            TakeDamage();
            return;
        }

        if (collision.gameObject.layer == volcanoLayer)
        {
            volcano = collision.GetComponent<Volcano>();
            if (volcano)
            {
                volcano.Erupt();
                charController.Volcano();
                volcano = null;
                return;
            }

            stayActiveVolcano = collision.GetComponent<StayActiveVolcano>();
            if (stayActiveVolcano)
            {
                stayActiveVolcano.Erupt();
                charController.Volcano();
                stayActiveVolcano = null;
                return;
            }
        }

        if (collision.gameObject.layer == levelEndLayer)
        {
            levelEnd = collision.GetComponent<LevelEnd>();
            if (levelEnd)
            {
                levelEnd.EndLevel();
                levelEnd = null;
                return;
            }
        }
    }

    ICollectable collectable;
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == collectableLayer)
        {
            collectable = InterfaceUtility.GetInterface<ICollectable>(collision.gameObject);
            if (collectable != null)
            {
                collectable.Collect(this);
                CamShake.Shake(0.2f, 1);
                collectable = null;
                return;
            }
        }
    }
}
