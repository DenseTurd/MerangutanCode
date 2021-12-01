using UnityEngine;

public class LinearPlatformController : MonoBehaviour
{
    public LinearPlatform linearPlatform;

    public void OnValidate()
    {
        if (linearPlatform == null)
        {
            Debug.Log("Assigning linear platform");
            linearPlatform = GetComponentInChildren<LinearPlatform>();
            if (linearPlatform == null)
            {
                Debug.Log("Cant find linear moving platform, make sure a child of the parent object has a LinearPlatform component attached");
                return;
            }
        }
        linearPlatform.OnValidate();
    }
}
