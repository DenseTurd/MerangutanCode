using UnityEngine;

public class TimedSpikesController : MonoBehaviour
{
    public TimedSpikes timedSpikes;

    public void OnValidate()
    {
        if (timedSpikes == null)
        {
            Debug.Log("Assigning timed spikes");
            timedSpikes = GetComponentInChildren<TimedSpikes>();
            if (timedSpikes == null)
            {
                Debug.Log("Cant find timed spikes, make sure a child of the parent object has a TimedSpikes component attached");
                return;
            }
        }
    }
}
