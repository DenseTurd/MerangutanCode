using System;
using UnityEngine;

public class PufferRilla : Enemy
{
    const float startingScale = 1;
    float scale = 1;
    const float cooldown = 0.7f;
    float cooldownTimer;
    bool deflating;

    Vector2 startingPos;

    public ParticleSystem poofParticles;

    public override void Start()
    {
        startingPos = transform.position;
        base.Start();
    }

    void Update()
    {
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }
        if (active)
        {
            if (deflating)
            {
                scale -= Time.deltaTime * 3f;
                if (scale < 1.01f)
                {
                    scale = 1;
                    cooldownTimer = cooldown;
                    deflating = false;
                }
            }
            else
            {

                scale += Time.deltaTime * 12;
                if (scale > 3.99f)
                {
                    scale = 4;
                    cooldownTimer = cooldown;
                    deflating = true;
                }
            }
            Vector3 astonish = Astonish();
            transform.localScale = new Vector3(scale * astonish.x, scale * astonish.y, scale * astonish.z);
        }
    }

    Vector3 Astonish()
    {
        float t = scale / 4;

        float horScaling = (1 + Mathf.Sin(Mathf.Lerp(-16, 0, t) * Mathf.Deg2Rad));

        float x, y, z;
        x = horScaling;
        y = (1 + Mathf.Sin(Mathf.Lerp(22, 0, t) * Mathf.Deg2Rad));
        z = horScaling;

        return new Vector3(x, y, z);
    }

    public void Pop()
    {
        poofParticles.transform.parent = null;
        poofParticles.Play();

        Overseer.Instance.audioManager.enemyAudio.Death(this);

        transform.position = new Vector3(spawnPos.x, spawnPos.y - 10000, spawnPos.z);

        Overseer.Instance.audioManager.adaptiveMusicManager.EnemyDistanceCheck();

        active = false;
    }

    public override void ResetMe()
    {
        scale = startingScale;
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = startingPos;

        poofParticles.transform.parent = this.transform;
        poofParticles.transform.localPosition = Vector3.zero;
    }
}
