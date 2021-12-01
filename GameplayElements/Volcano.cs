using UnityEngine;

public class Volcano : MonoBehaviour, ISectionActivatable
{
    public ParticleSystem particleSystem;
    const float cooldown = 0.5f;
    float cooldownTimer;

    public void Erupt()
    {
        if (cooldownTimer <= 0)
        {
            particleSystem.Play();
            cooldownTimer = cooldown;
        }
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
