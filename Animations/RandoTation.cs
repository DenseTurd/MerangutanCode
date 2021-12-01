using UnityEngine;

public class RandoTation : MonoBehaviour
{ 
    Vector3 from;
    Vector3 to;
    Vector3 toward;
    float nextOrientationDelay;
    float nextOrientationTime;
    float step;
    float slerpStep;
    float slerpStepTime;
    public float speed = 2f;

    void Start()
    {
        SetNewOrientation();
    }

    void Update()
    {
        if (Time.time < nextOrientationTime)
        {
            toward = new Vector3(Mathf.Lerp(from.x, to.x, step), Mathf.Lerp(from.y, to.y, step), Mathf.Lerp(from.z, to.z, step));

            transform.Rotate(toward.z, toward.x, toward.y, Space.World);
            step += slerpStep * Time.deltaTime;
        }
        else
        {
            SetNewOrientation();
        }
    }

    void SetNewOrientation()
    {
        from = to;
        to = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), Random.Range(-speed, speed));
        nextOrientationDelay = Random.Range(0.3f, 6f);
        slerpStepTime = Random.Range(nextOrientationDelay, nextOrientationDelay * 1.2f);
        slerpStep = 1 / slerpStepTime;
        step = 0f;
        nextOrientationTime = Time.time + nextOrientationDelay;
    }
}
