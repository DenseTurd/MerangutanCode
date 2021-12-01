using System;
using UnityEngine;

public class BreakingPlatform : MovingPlatform
{
    public Transform home;
    public Transform breakingPoint;
    [HideInInspector] public Vector3 offset = new Vector3(0, 0, 1);
    [HideInInspector] public float distance;
    [HideInInspector] public Vector3 lifeTime = new Vector3(1.5f, 0.2f, 5f);

    [HideInInspector] public BreakingPlatformController controller;

    public bool breaking;
    public bool singleUse;

    void Start()
    {
        frequency.x = 1 / lifeTime.x;
        angle = offset.x * 360;
        distance = Vector2.Distance(home.position, breakingPoint.position);
    }

    public override void FixedUpdate()
    {
        if (breaking)
            base.FixedUpdate();
    }

    public override void Move(float angle)
    {
        if (breaking)
        {
            if (angle >= 330)
                controller.Despawn();

            float T = angle / 360;

            transform.position = Vector3.Lerp(home.position, breakingPoint.position, T);
        }
    }

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}
