using UnityEngine;

public class ZSwingingDanger : MonoBehaviour, ISectionActivatable
{
    public Transform gfx;
    public GameObject failArea;

    public Vector3 frequency = new Vector3(0.8f, 0.05f, 5f);
    public Vector3 offset = new Vector3(0, 0, 1);

    float angle;
    void Start()
    {
        angle = offset.x * 360;
    }

    void Move(float angle)
    {
        float T = Mathf.Sin(Mathf.Deg2Rad * angle);
        T = T.EaseOut(); // Radians! it's always the radians! that's a nice easing function tho :D
        T = T.EaseOut();
        T *= 60;
        gfx.rotation = Quaternion.Euler(new Vector3(T, gfx.rotation.y, gfx.rotation.z));
    }

    void Update()
    {
        angle += Time.deltaTime * frequency.x * 360;
        if (angle >= 360)
            angle -= 360;

        Move(angle);

        if (angle > 345 || angle < 15 || angle > 165 && angle < 195)
        {
            failArea.SetActive(true);
        }
        else
        {
            failArea.SetActive(false);
        }
    }

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}
