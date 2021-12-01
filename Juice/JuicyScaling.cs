using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicyScaling : MonoBehaviour
{
    Vector3 originalScale;
    bool boinging;

    const float timerTime = 0.3f;
    float timer;

    void Start()
    {
        originalScale = transform.localScale;
    }
    public void Boing()
    {
        boinging = true;
    }

    void Update()
    {
        if (boinging)
        {
            float t = (1 / timerTime) * timer;

            float horScaling = (1 + Mathf.Sin(Mathf.Lerp(-16, 0, t) * Mathf.Deg2Rad));

            float x, y, z;
            x = originalScale.x * horScaling;
            y = originalScale.y * (1 + Mathf.Sin(Mathf.Lerp(22, 0, t) * Mathf.Deg2Rad));
            z = originalScale.z * horScaling;

            Vector3 scale = new Vector3(x, y, z);

            timer += Time.deltaTime;
            if (timer >= timerTime)
            {
                boinging = false;
                timer = 0;
                scale = originalScale;
            }

            transform.localScale = scale;
        }    
    }
}
