using UnityEngine;

public class DTCoin : Spawnable, ICollectable, ISectionActivatable
{
    public float maxPursueSpeed = 45;
    float speed = 1;
    bool pursuit;
    float pursuitTimer = 0.3f;
    public ParticleSystem coinPoof;
    public Transform mesh;
    Vector3 rot = Vector3.zero;
    const float rotateTime = 0.1f;
    float rotateTimer;
    float spin = 720;
    public Player _player;
    float autoCollectTimer = 2f;
    bool spinnySoundTriggered;

    public void Collect(Player player)
    {
        if (pursuitTimer <= 0)
        {
            CollectForReal();
        }
        _player = player;
        coinPoof.Play();
        pursuit = true;
    }

    void CollectForReal()
    {
        coinPoof.transform.parent = null;
        coinPoof.Play();
        DTPrefs.IncrementInt(DTPrefs.GetString(Strs.playerID) + Strs.DTCoins);
        Overseer.Instance.hud.UpdateCoinsVal();

        Overseer.Instance.audioManager.collectiblesAudio.CollectCoin();

        Destroy(gameObject);
    }

    void Update()
    {
        if (!pursuit)
        {
            VintageSpin();
        }
        else
        {
            SpinnySound();
            Spinny();
            Pursue();
            AutoCollect();
        }
    }

    void SpinnySound()
    {
        if (!spinnySoundTriggered)
        {
            spinnySoundTriggered = true;
            Overseer.Instance.audioManager.collectiblesAudio.SpinCoin();
        }
    }

    void VintageSpin()
    {
        rotateTimer -= Time.deltaTime;
        if (rotateTimer <= 0)
        {
            rotateTimer = rotateTime;
            Rotate(22.5f);
        }
    }

    void Spinny()
    {
        Rotate(spin * Time.deltaTime * 10);
        spin *= 1 - (Time.deltaTime * 5);
    }

    void Rotate(float amount)
    {
        rot = new Vector3(rot.x, rot.y + amount, rot.z);
        mesh.rotation = Quaternion.Euler(rot);
    }

    void Pursue()
    {
        pursuitTimer -= Time.deltaTime;
        if (pursuitTimer <= 0)
        {
            Vector3 dir = (_player.transform.position - transform.position).normalized;

            transform.Translate(dir * speed * Time.deltaTime);

            speed += speed * Time.deltaTime * 15;
            speed = speed > maxPursueSpeed ? maxPursueSpeed : speed;
        }
    }

    void AutoCollect()
    {
        autoCollectTimer -= Time.deltaTime;
        if (autoCollectTimer <= 0)
        {
            transform.position = _player.transform.position;
            CollectForReal();
        }
    }
}
