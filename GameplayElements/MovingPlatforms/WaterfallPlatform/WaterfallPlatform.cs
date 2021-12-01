using UnityEngine;

public class WaterfallPlatform : MovingPlatform
{
    public Transform pointA;
    public Transform pointB;
    [HideInInspector] public Vector3 offset = new Vector3(0, 0, 1);

    [HideInInspector] public float distance;

    void Start()
    {
        angle = offset.x * 360;
        distance = Vector2.Distance(pointA.position, pointB.position);
    }

    public override void Move(float angle)
    {
        float T = angle / 360;

        transform.position = Vector3.Lerp(pointA.position, pointB.position, T);
    }

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}
