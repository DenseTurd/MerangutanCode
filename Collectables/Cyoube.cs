using UnityEngine;

public class Cyoube : Spawnable, ICollectable, ISectionActivatable
{
    public ParticleSystem starPoof;
    public void Collect(Player player)
    {
        starPoof.transform.parent = null;
        starPoof.Play();
        Overseer.Instance.player.Heal(1);
        Destroy(gameObject);
    }
}
