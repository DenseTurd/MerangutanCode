using UnityEngine;

[RequireComponent(typeof(ProximityActivator))]
public class CheckPoint : Spawnable, IProximityActivatable
{
    public RandoTation randoTation;
    public Bobbing bobbing;

    void Start()
    {
        Overseer.Instance.checkPoints.checkPoints.Add(this);
        transform.parent = Overseer.Instance.checkPoints.transform;
    }

    public void Activate()
    {
        if (!randoTation.enabled)
        {
            Overseer.Instance.checkPoints.AssignCurrentCheckPoint(this);
            Overseer.Instance.audioManager.playerAudio.Checkpoint();
        }
    }

    public void BeTheCurrentCheckPoint()
    {
        randoTation.enabled = true;
        bobbing.enabled = true;
    }

    public void StopBeingTheCurrentCheckPoint()
    {
        randoTation.enabled = false;
        bobbing.enabled = false;
    }

    public void DeActivate()
    {
        // not needed, only here to satisfy the interface requirement
    }
}
