using UnityEngine;

public class Bobbing : MonoBehaviour
{
    // Mathf.Sin is in Radians ya berk!
    // There are 2pi radians in 360 degrees
    // You can get the constants in the Mathf class i.e Mathf.RadToDeg
    public float bobTime = 1f;
    public float bobHeight = 0.5f;
    Vector3 originalPos;
    float step;
    float t;
    float y;
    void Start()
    {
        originalPos = transform.localPosition;
        step = (Mathf.PI * 2) * (1 / bobTime);
    }

    void Update()
    {

        t += step * Time.deltaTime;
        if (t >= Mathf.PI * 2)
        {
            t -= Mathf.PI * 2;
        }
        y = Mathf.Sin(t);

        transform.localPosition = new Vector3(originalPos.x, originalPos.y + (y * bobHeight), originalPos.z);
    }
}
