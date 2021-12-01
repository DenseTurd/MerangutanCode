using UnityEngine;

public class ProximityActivator : MonoBehaviour
{
    [HideInInspector]
    public IProximityActivatable activatable;

    [Range(0.1f, 40)]
    public float maxActiveDist = 10f;
    const float standbyCheckTime = 0.3f;
    float standbyCheckTimer;
    bool standingBy;
    public float currentDistance;

    void OnEnable()
    {
        activatable = this.GetComponentOrComplain<IProximityActivatable>();
    }

    void Update()
    {
        standbyCheckTimer -= Time.deltaTime;
        if (standbyCheckTimer <= 0)
        {
            if (DistCheck(maxActiveDist * 2))
            {
                standingBy = true;
            }
            else
            {
                standingBy = false;
            }
            standbyCheckTimer = standbyCheckTime;
        }

        if (standingBy)
        {
            if (DistCheck(maxActiveDist))
            {
                activatable.Activate();
            }
            else
            {
                activatable.DeActivate();
            }
        }
        
    }

    bool DistCheck(float dist)
    {
        currentDistance = Vector2.Distance(transform.position, Overseer.Instance.player.transform.position);

        if (currentDistance < dist)
        {
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, maxActiveDist);
    }
#endif
}
