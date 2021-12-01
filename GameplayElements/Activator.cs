using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject toActivate;
    public bool deactivate;

    void OnTriggerEnter2D() => toActivate.SetActive(!deactivate);
}