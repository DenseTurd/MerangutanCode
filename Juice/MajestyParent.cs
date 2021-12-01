using UnityEngine;

public class MajestyParent : MonoBehaviour
{
    public void StartMajesty()
    {
        foreach (Transform child in transform)
        {
            //child.GetComponent<MajestyStarSystem>().enabled = true;
        }
    }

    public void StopMajesty()
    {
        foreach (Transform child in transform)
        {
            //child.GetComponent<MajestyStarSystem>().enabled = false;
        }
    }
}
