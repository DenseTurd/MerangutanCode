using UnityEngine;

public class CharAnimation : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = this.GetComponentOrComplain<Animator>();
    }

    public void Run(bool val)
    {
        anim.SetBool("Run", val);
    }
}
