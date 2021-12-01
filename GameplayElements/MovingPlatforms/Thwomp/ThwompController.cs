using UnityEngine;

[RequireComponent(typeof(ProximityActivator))]
public class ThwompController : MonoBehaviour, IProximityActivatable
{
    public Thwomp thwomp;

    public void OnValidate()
    {
        if (thwomp == null)
        {
            Debug.Log("Assigning thwomp");
            thwomp = GetComponentInChildren<Thwomp>();
            if (thwomp == null)
            {
                Debug.Log("Cant find thwomp, make sure a child of the parent object has a Thwomp component attached");
                return;
            }
        }
        thwomp.OnValidate();
    }

    public void Activate()
    {
        thwomp.Activate();
    }

    public void DeActivate()
    {
        thwomp.DeActivate();
    }
}
