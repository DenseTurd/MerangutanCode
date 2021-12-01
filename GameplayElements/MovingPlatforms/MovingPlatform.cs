using UnityEngine;

public abstract class MovingPlatform : MonoBehaviour, ISectionActivatable
{
    [HideInInspector] public Vector3 frequency = new Vector3(0.5f, 0.1f, 2);

    [HideInInspector] public float angle;

    [HideInInspector] public Vector3 deltaPos;
    [HideInInspector] public Vector3 deltaVelocity;
    [HideInInspector] public Vector3 prevVeloctiy;

    public virtual void FixedUpdate()
    {
        Vector3 prevPos = transform.position;

        angle += Time.deltaTime * frequency.x * 360;
        if (angle > 360) angle -= 360;
        
        Move(angle);

        deltaPos = transform.position - prevPos;

        deltaVelocity = deltaPos - prevVeloctiy;
        prevVeloctiy = deltaPos;
    }

    public abstract void Move(float T);

    public Vector3 DeltaPos()
    {
        return deltaPos;
    }

    public Vector2 DeltaVelocity()
    {
        return deltaVelocity;
    }
}
