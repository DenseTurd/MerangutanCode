using UnityEngine;

public class LinearPlatform : MovingPlatform
{
    public Transform pointA;
    public Transform pointB;
    [HideInInspector] public Vector3 offset = new Vector3(0, 0, 1);

    void Start()
    {
        angle = offset.x * 360;
    }

    public override void Move(float angle)
    {
        float T = Mathf.Sin(angle * Mathf.Deg2Rad);
        T += 1;
        T *= 0.5f;
        transform.position = Vector3.Lerp(pointA.position, pointB.position, T);
    }

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}
