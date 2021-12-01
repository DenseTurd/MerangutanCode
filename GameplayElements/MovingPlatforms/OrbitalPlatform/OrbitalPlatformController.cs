using UnityEngine;

public class OrbitalPlatformController : MonoBehaviour
{
    public OrbitalPlatform orbitalPlatform;

    public void OnValidate()
    {
        if (orbitalPlatform == null)
        {
            Debug.Log("Assigning orbital platform");
            orbitalPlatform = GetComponentInChildren<OrbitalPlatform>();
            if (orbitalPlatform == null)
            {
                Debug.Log("Cant find orbital platform, make sure a child of the centre object has a OrbitalPlatform component attached");
                return;
            }
        }
        orbitalPlatform.OnValidate();
    }
}
