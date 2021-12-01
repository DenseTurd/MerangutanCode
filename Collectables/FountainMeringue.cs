using UnityEngine;

public class FountainMeringue : Spawnable, ICollectable, ISectionActivatable
{
    public ParticleSystem starPoof;
    public void Collect(Player player)
    {
        starPoof.transform.parent = null;
        starPoof.Play();
        Overseer.Instance.player.Heal(1);
        transform.parent.gameObject.GetComponent<ParticleController>().ReturnToPool();
    }

    void OnEnable()
    {
        starPoof.transform.parent = transform;
        starPoof.transform.localPosition = Vector3.zero;
        starPoof.transform.localScale = Vector3.one;
    }
}
