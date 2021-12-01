using UnityEngine;

public  class Graboon : Enemy
{
    public ParticleSystem poofParticles;
    public Animator anim;

    public override void Start()
    {
        anim = this.GetComponentOrComplain<Animator>();
        base.Start();
    }

    void Update()
    {
        if (active)
        {
            anim.SetBool(Strs.active, true);
        }
        else
        {
            anim.SetBool(Strs.active, false);
        }
    }

    public override void ResetMe()
    {
        // only here to satisfy inheritance
    }
}
