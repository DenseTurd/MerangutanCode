using UnityEngine;

public class OrbitalPlatform : MovingPlatform
{
    public Transform centre;
    public Vector3 radius = new Vector3(4, 0.5f, 10f);
    public Vector3 startingAngle = new Vector3(0, 0, 359);
    public bool clockwise = true;

    void Start()
    {
        angle = clockwise? startingAngle.x : -startingAngle.x;    
    }

    public override void Move(float angle)
    {
        float X = Mathf.Sin((clockwise ? angle : -angle) * Mathf.Deg2Rad);
        float Y = Mathf.Cos((clockwise ? angle : -angle) * Mathf.Deg2Rad);

        transform.position = new Vector3(
            centre.position.x + (X * radius.x),
            centre.position.y + (Y * radius.x),
            centre.position.z
            );
    }

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}


