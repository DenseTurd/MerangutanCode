using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    [Header("Up, Down, left, right")]
    public KeyCode up = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode down = KeyCode.S;
    public KeyCode right = KeyCode.D;

    [Space]
    [Header("Character abilities")]
    public KeyCode jump = KeyCode.Mouse0;
    public KeyCode swing = KeyCode.Space;
    public KeyCode dash = KeyCode.Mouse1;

    [Space]
    [Header("Menu navigation")]
    public KeyCode next = KeyCode.E;
    public KeyCode yes = KeyCode.Y;
    public KeyCode no = KeyCode.N;

    public KeyCode pause = KeyCode.Escape;

    CharController controller;
    Swinger swinger;

    float horizontalMove;
    float verticalMove;
    Vector2 move;

    bool jumpPressedThisFixedUpdate;
    bool jumping;
    bool holdingJump;
    bool dashing;

    private void Start()
    {
        controller = this.GetComponentOrComplain<CharController>();
        swinger = this.GetComponentOrComplain<Swinger>();
        move = new Vector2(0, 0);
    }

    void Update()
    {
        //horizontalMove = HorizontalAxis();
        //verticalMove = VerticalAxis();
        //move = new Vector2(horizontalMove, verticalMove);

        //jumping = Input.GetKeyDown(jump) || jumping; // Needs the or to stop jump getting set back to false in the same FixedUpdate frame
        //holdingJump = Input.GetKey(jump) || holdingJump; // Same here 
        
        //dashing = Input.GetKeyDown(dash) || dashing; // Same here

        //if (Input.GetKeyDown(next)) 
        //if (Input.GetKeyDown(yes)) Overseer.Instance.dialogueManager.Yes();
        //if (Input.GetKeyDown(no)) Overseer.Instance.dialogueManager.No();

        //if (Input.GetKeyDown(pause)) Overseer.Instance.pauseMenu.Pause();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) jumpPressedThisFixedUpdate = true;
        if (context.phase == InputActionPhase.Performed) holdingJump = true;
        if (context.phase == InputActionPhase.Canceled) holdingJump = false;
    }

    public void Swing(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) swinger.StartSwing();
        if (context.phase == InputActionPhase.Canceled) swinger.StopSwing();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) dashing = true;
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) Overseer.Instance.dialogueManager.NextSentence();
    }

    public void Yes(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) Overseer.Instance.dialogueManager.Yes();
    }

    public void No(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) Overseer.Instance.dialogueManager.No();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) Overseer.Instance.pauseMenu.Pause();
    }

    float HorizontalAxis()
    {
        float lefty = Input.GetKey(left) ? -1 : 0;
        float righty = Input.GetKey(right) ? 1 : 0;
        return lefty + righty;
    }

    float VerticalAxis()
    {
        float uppy = Input.GetKey(up) ? 1 : 0;
        float downy = Input.GetKey(down) ? -1 : 0;
        return uppy + downy;
    }

    void FixedUpdate()
    {
        jumping = jumpPressedThisFixedUpdate;
        controller.Move(move, dashing, jumping, holdingJump);
        jumpPressedThisFixedUpdate = false;
        jumping = false;
        dashing = false;
    }
}
