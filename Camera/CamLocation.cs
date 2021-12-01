using UnityEngine;

[RequireComponent(typeof(ProximityActivator))]
public class CamLocation : MonoBehaviour, IProximityActivatable, ISectionActivatable
{
    public Transform desiredLocation;
    CamOffsetter offsetter;

    ProximityActivator proximityActivator;
    bool active;

    Vector3 relativePosition;

    void Start()
    {
        proximityActivator = this.GetComponentOrComplain<ProximityActivator>();
        offsetter = Overseer.Instance.camManager.camOffsetter;
    }

    float t;
    void Update()
    {
        if (active)
        {
            if (!offsetter) return;
            relativePosition = desiredLocation.position - Camera.main.transform.position;
            t = (-(Vector2.Distance(transform.position, Overseer.Instance.player.transform.position)) + proximityActivator.maxActiveDist) / proximityActivator.maxActiveDist;
            offsetter.desiredOffsets.Add(Vector3.Lerp(offsetter.originalOffset, relativePosition, t));
        }    
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
        }
    }

    public void DeActivate()
    {
        if (active)
        {
            active = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(desiredLocation.position, 0.5f);
    }
#endif
}
