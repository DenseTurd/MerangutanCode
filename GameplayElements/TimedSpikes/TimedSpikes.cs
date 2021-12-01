using UnityEngine;

public class TimedSpikes : MonoBehaviour, ISectionActivatable
{
    [HideInInspector] public Vector3 spikeTime = new Vector3(1f, 0.1f, 5f);
    [HideInInspector] public Vector3 safeTime = new Vector3(1.5f, 0.1f, 5f);
    public Transform retractedPos;
    Vector3 extendedPos;
    float timer;
    bool spikes;

    float maxDistDelta;

    void Start()
    {
        extendedPos = transform.position;
        maxDistDelta = Vector3.Distance(extendedPos, retractedPos.position);       
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (spikes)
        {
            if (timer <= 0)
            {
                spikes = false;
                timer = safeTime.x;
            }

            if (transform.position != extendedPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, extendedPos, maxDistDelta * Time.deltaTime * 15);
            }
        }
        else
        {
            if (timer <= 0)
            {
                spikes = true;
                timer = spikeTime.x;
            }

            if (transform.position != retractedPos.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, retractedPos.position, maxDistDelta * Time.deltaTime * 15);
            }
        }
    }
}
