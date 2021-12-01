using System;
using UnityEngine;

public class NevilleAI : MonoBehaviour
{
    [Header("Up, Down, left, right")]
    public bool up;
    public bool left;
    public bool down;
    public bool right;

    [Space]
    [Header("Character abilities")]
    public bool jump;
    public bool swing;
    public bool dash;


    NiallsCharController controller;
    Swinger swinger;

    float horizontalMove;
    float verticalMove;
    Vector2 move;

    bool jumping;
    bool holdingJump;
    bool dashing;

    float hMove = 1;

    const float canJumpTime = 0.5f;
    float canJumpTimer;

    bool stuckOnAWall;

    Vector2 prevFramePos = Vector2.zero;
    Vector2 prevJumpPos = Vector2.zero;

    private void Start()
    {
        controller = this.GetComponentOrComplain<NiallsCharController>();
        swinger = this.GetComponentOrComplain<Swinger>();
    }

    void Update()
    {
        Senses();
        horizontalMove = HorizontalAxis();
        verticalMove = VerticalAxis();
        move = new Vector2(horizontalMove, verticalMove);

        if (canJumpTimer >= 0)
        {
            canJumpTimer -= Time.deltaTime;
        }

        prevFramePos = transform.position;
    }

    bool OnAWall()
    {
        stuckOnAWall = transform.position.x == prevJumpPos.x ? true : false;
        prevJumpPos = transform.position;
        Debug.Log($"Stuck on a wall: {stuckOnAWall}");
        return prevFramePos.x == transform.position.x;
    }

    void Senses()
    {
        if (Sense(Vector2.right, 2)) // looking forward
        {
            Jump();
        }


        if (Sense(new Vector2(0, -1), 0.3f)) // esentially a ground check
        {
            if (!Sense(new Vector2(1, -0.5f), 2)) // looking diagonally down
            {
                Jump();
                if (hMove < 0)
                {
                    Reverse();
                }
            }
        }
    }

    public void Jump()
    {
        if (!jumping)
        {
            if (canJumpTimer <= 0)
            {
                if (OnAWall())
                {
                    if (hMove < 0)
                    {
                        Reverse();
                    }
                    if (stuckOnAWall)
                    {
                        Reverse();
                    }
                }
                jumping = true;
                canJumpTimer = canJumpTime;
            }
        }
    }

    void Reverse()
    {
        hMove = hMove * -1;
    }

    bool Sense(Vector2 dir, float length)
    {
        bool rasult = false;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y - 0.75f);
        Color col = Color.green;
        int layerMask = 1 << 6;

        Vector2 flippedDir = new Vector2(dir.x * (controller.facingRight ? 1 : -1), dir.y);

        RaycastHit2D hit = Physics2D.Raycast(pos, flippedDir, length, layerMask);
        if (hit)
        {
            rasult = true;
            col = Color.red;
        }
        Debug.DrawRay(pos, flippedDir * length, col);
        return rasult;
    }

    void FixedUpdate()
    {
        controller.Move(move, dashing, jumping, holdingJump);
        jumping = false;
        holdingJump = false;
        dashing = false;
    }

    float HorizontalAxis()
    {
        //float lefty = Key.DoesWhen(left, When.Held) ? -1 : 0;
        //float righty = Key.DoesWhen(right, When.Held) ? 1 : 0;
        //return lefty + righty;

        return hMove;
    }

    float VerticalAxis()
    {
        //float uppy = Key.DoesWhen(up, When.Held) ? 1 : 0;
        //float downy = Key.DoesWhen(down, When.Held) ? -1 : 0;
        //return uppy + downy;

        return 1;
    }
}
