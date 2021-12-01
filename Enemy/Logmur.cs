using UnityEngine;

public  class Logmur : Enemy
{
    public ParticleSystem poofParticles;
    public GameObject logParent;
    public GameObject logRoot;
    public GameObject failArea;
    float angle;

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!active) return;

        angle += Time.deltaTime * 270;
        if (angle > 360)
        {
            angle -= 360;
        }

        logParent.transform.rotation = Quaternion.Euler(0, angle, 0);

        Ray ray = new Ray(logRoot.transform.position, logRoot.transform.forward);
        Plane zPlane = new Plane(Vector3.forward, Vector3.zero);
        if (zPlane.Raycast(ray, out float dist))
        {
            if (dist <= 8)
            {
                failArea.SetActive(true);
                failArea.transform.position = ray.GetPoint(dist);
            }
            else
            {
                failArea.SetActive(false);
            }
        }
        else
        {
            failArea.SetActive(false);
        }
    }

    public override void ResetMe()
    {
        // only here to satisfy inheritance
    }
}
