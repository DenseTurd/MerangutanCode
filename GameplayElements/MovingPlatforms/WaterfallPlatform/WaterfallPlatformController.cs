using UnityEngine;

public class WaterfallPlatformController : MonoBehaviour
{
    public WaterfallPlatform waterfallPlatform;

    public void OnValidate()
    {
        if (waterfallPlatform == null)
        {
            Debug.Log("Assigning waterfall platform");
            waterfallPlatform = GetComponentInChildren<WaterfallPlatform>();
            if (waterfallPlatform == null)
            {
                Debug.Log("Cant find waterfall platform, make sure a child of the parent object has a WaterfallPlatform component attached");
                return;
            }
        }
        waterfallPlatform.OnValidate();
    }
}
