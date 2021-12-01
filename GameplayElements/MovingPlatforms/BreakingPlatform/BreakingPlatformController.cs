using UnityEngine;

public class BreakingPlatformController : MonoBehaviour
{
    public BreakingPlatform breakingPlatform;

    public Vector3 respawnTime = new Vector3(3f, 0.1f, 5f);
    float respawnTimer;
    bool respawning;

    public void OnValidate()
    {
        if (breakingPlatform == null)
        {
            Debug.Log("Assigning breaking platform");
            breakingPlatform = GetComponentInChildren<BreakingPlatform>();
            if (breakingPlatform == null)
            {
                Debug.Log("Cant find breaking platform, make sure a child of the parent object has a BreakingPlatform component attached");
                return;
            }
        }
        breakingPlatform.controller = this;
        breakingPlatform.OnValidate();
    }

    public void Despawn()
    {
        breakingPlatform.gameObject.SetActive(false);
        respawnTimer = respawnTime.x;
        respawning = true;
    }

    void Update()
    {
        if (respawning)
        {
            if (breakingPlatform.singleUse)
            {
                ResetPlatform();
                gameObject.SetActive(false);
            }

            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                ResetPlatform();
            }
        }
    }

    void ResetPlatform()
    {
        breakingPlatform.angle = breakingPlatform.offset.x;
        breakingPlatform.breaking = false;
        breakingPlatform.transform.position = breakingPlatform.home.position;
        //breakingPlatform.OnValidate();
        breakingPlatform.gameObject.SetActive(true);
        respawning = false;
    }
}
