using UnityEngine;

[System.Serializable]
public class Triggerer : MonoBehaviour
{
    public INotifyable notifyable;
    ParticleSystem particleSystem;
    bool notified;

    void Start()
    {
        particleSystem = this.GetComponentOrComplain<ParticleSystem>();    
    }
    void OnTriggerEnter2D()
    {
        if (!notified)
        {
            notifyable.Notify();
            particleSystem.Play();
            notified = true;
        }
    }
}
