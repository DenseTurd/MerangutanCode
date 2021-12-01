using UnityEngine;

public class CharController : MonoBehaviour
{
    #region vars
    public float maxHorizontalSpeed = 12;
    public float groundAccel = 120f;
    public float airAccel = 46;
    public float swingAccel = 18;
    [SerializeField] float jumpForce = 3000f;                         
    [SerializeField] float variableJumpForce = 31;                    // Amount of extra force to apply to vary the jump height 
	[SerializeField] float groundDrag = 20f;                          // Drag to use when grounded with no horizontal input
	[SerializeField] float airDrag = 1f;                              // Drag to use when in air with no input
	[SerializeField] public LayerMask whatIsGround;                   // A mask determining what is ground to the character
	[SerializeField] Transform groundCheck;                           // Where to check if the player is grounded.
	[SerializeField] Transform ceilingCheck;                          // Where to check for ceilings
    [SerializeField] Transform wallCheck;                             // For checking if we can wall jump

	[HideInInspector] public bool swinging;
    float wasSwingingTimer;
    float swingJumpCooldownTime = 0.5f;
    float swingJumpCooldownTimer;
    bool coyote;
    float coyoteTime = 0.15f;
    float coyoteTimer;
    bool canBounce;
    const float bounceTime = 0.2f;
    float bounceTimer;
    float variableJumpHeightTime = 0.5f;
    float variableJumpHeightTimer;
    bool dashing;
    [HideInInspector] 
    public bool canDash;
    float dashTimer;
    const float dashTime = 0.15f;
    bool dashRight;
    float dashSpeed = 1100;
    bool canAirJump;
    bool landing;

    bool wallSlide;
    float wallSlideSpeed = -5f;
    float wallSlideCooldownTime = 0.1f;
    float wallSlideCooldownTimer;

	//const float groundedRadius = .35f; // Radius of the overlap circle to determine if grounded
    Vector2 groundCheckBox = new Vector2(0.75f, 0.45f); // box to determine if grounded is better for Merangutan

    const float wallCheckRadius = 0.15f; // same for wall slides
	[HideInInspector] public bool grounded;
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public bool facingRight = true; 
    [HideInInspector] bool wasDashing = false;

    Vector2 prevVelocity;

    public JuicyScaling juicyScaling;
    public ParticleSystem poofParticles;
    public ParticleSystem dashParticles;

    [HideInInspector] public CharAnimation animation;
    #endregion

    void Awake()
	{
		rb2D = this.GetComponentOrComplain<Rigidbody2D>();
        animation = GetComponentInChildren<CharAnimation>();
        colliders = new Collider2D[64];
        wallColliders = new Collider2D[64];
    }

    void FixedUpdate()
    {
        GroundCheck();
        CoyoteTime();
        BounceTime();
        WasSwinging();
        SwingJumpCooldown();
        VariableJumpHeight();
        Dash();
        WallSlideCooldownTimer();

        prevVelocity = rb2D.velocity;

        EruptTimer();
    }

    Collider2D[] colliders;
    MovingPlatform movingPlatform;
    public void GroundCheck()
    {
        bool wasGrounded = grounded;
        grounded = false;

        int GroundlayerMask = 1 << 6; // thoe ole bitshift trick :s
        int noOfColliders = Physics2D.OverlapBoxNonAlloc(groundCheck.position, groundCheckBox, 0, colliders,  GroundlayerMask);
        for (int i = 0; i < noOfColliders; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                canDash = true;
                canAirJump = false;

                if (colliders[i].CompareTag(Strs.movingPlatform))
                {
                    movingPlatform = colliders[i].gameObject.GetComponent<MovingPlatform>();
                    if (movingPlatform) MovingPlatforms(movingPlatform);
                }
            }
        }

        if (wasGrounded && !grounded) // it's coyote time!
        {
            if (!coyote)
            {
                coyote = true;
                coyoteTimer = coyoteTime;
            }
        }

        if (!wasGrounded && grounded) // window for a bounce jump
        {
            if (!canBounce)
            {
                canBounce = true;
                bounceTimer = bounceTime;
                if (prevVelocity.y < -20) CamShake.Shake(0.2f, prevVelocity.y / 10);

                poofParticles.Play();

                if (prevVelocity.y < 0)
                {
                    Overseer.Instance.audioManager.playerAudio.Land();
                }
            }
        }
    }

    void CoyoteTime()
    {
        if (coyote)
        {
            coyoteTimer -= Time.deltaTime;

            if (coyoteTimer <= 0)
            {
                coyote = false;
            }
        }
    }

    void BounceTime()
    {
        if (canBounce)
        {
            bounceTimer -= Time.deltaTime;

            if (bounceTimer <= 0)
            {
                canBounce = false;
            }
        }
    }

    void WasSwinging()
    {
        if (swinging)
        {
            wasSwingingTimer = coyoteTime;
        }
        else
        {
            if (wasSwingingTimer >= 0)
            {
                wasSwingingTimer -= Time.deltaTime;
            }
        }
    }

    void SwingJumpCooldown()
    {
        if (swingJumpCooldownTimer > 0)
        {
            swingJumpCooldownTimer -= Time.deltaTime;
        }
    }

    void VariableJumpHeight()
    {
        if (variableJumpHeightTimer >= 0)
        {
            variableJumpHeightTimer -= Time.deltaTime;
        }
    }

    void Dash()
    {
        if (dashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashing = false;
            }
            rb2D.velocity = new Vector2(dashSpeed * Time.deltaTime * (dashRight ? 1 : -1), rb2D.velocity.y * 0.5f * Time.deltaTime);
        }
    }

    void WallSlideCooldownTimer()
    {
        if (wallSlideCooldownTimer >= 0)
        {
            wallSlideCooldownTimer -= Time.deltaTime;
        }
    }

    public void Move(Vector2 move, bool dash, bool jump, bool holdingJump)
    {
        if (grounded)
        {
            GroundControl(move);
        }
        else if (!swinging) // Air control
        {
            AirControl(move);
        }

        SortDash(dash);
        SwingInfluence(move);
        SortDrag(move, holdingJump);
        RestrictSpeed();
        Jump(jump, move);
        VaryJumpHeight(holdingJump, move);
        FlipPlayer(move);
        WallSlide(move);
    }

    void GroundControl(Vector2 move)
    {
        if (!dashing)
        {
            if (move.x != 0 && !swinging) // Ground control
            {
                animation.Run(true);
                // only control up to max speed 
                if (move.x < 0 && rb2D.velocity.x > -maxHorizontalSpeed || move.x > 0 && rb2D.velocity.x < maxHorizontalSpeed)
                {
                    float targetVelocityX = rb2D.velocity.x + ((move.x * groundAccel) * Time.fixedDeltaTime);
                    rb2D.velocity = new Vector2(targetVelocityX, rb2D.velocity.y);
                }
            }
            else
            {
                animation.Run(false);
            }
        }
    }

    void AirControl(Vector2 move)
    {
        if (move.x != 0)
        {
            // only control up to max speed (max speed can be exceeded by swings)
            if (move.x < 0 && rb2D.velocity.x > -maxHorizontalSpeed || move.x > 0 && rb2D.velocity.x < maxHorizontalSpeed)
            {
                float targetVelocityX = rb2D.velocity.x + ((move.x * airAccel) * Time.fixedDeltaTime);
                rb2D.velocity = new Vector2(targetVelocityX, rb2D.velocity.y);
            }
        }
    }

    void SortDash(bool dash)
    {
        if (dash)
        {
            if (!wasDashing)
            {
                wasDashing = true;
                StartDash();
            }
        }
        else
        {
            if (wasDashing)
            {
                wasDashing = false;
            }
        }
    }

    void StartDash()
    {
        if (canDash)
        {
            dashing = true;
            canDash = false;
            canAirJump = true;
            dashTimer = dashTime;
            dashRight = facingRight;

            dashParticles.Play();

            Overseer.Instance.audioManager.playerAudio.DashJump();
        }
    }

    void SwingInfluence(Vector2 move)
    {
        if (swinging)
        {
            Vector3 targetVelocity = rb2D.velocity + ((move * swingAccel) * Time.fixedDeltaTime);
            rb2D.velocity = targetVelocity;
        }
    }

    void SortDrag(Vector2 move, bool holdingJump)
    {
        if (swinging)
        {
            rb2D.drag = 0;
            return;
        }

        if (dashing)
        {
            rb2D.drag = 0;
            return;
        }

        if (holdingJump) // if jumping
        {
            rb2D.drag = 0;
            return;
        }

        if (move.x != 0) // if left or right
        {
            rb2D.drag = 0;
            return;
        }

        if (move.x == 0 && grounded) // if no left or right while on ground
        {
            rb2D.drag = groundDrag;
        }

        if (move.magnitude == 0 && !grounded) // if no input in air
        {
            rb2D.drag = airDrag;
        }

    }

    void RestrictSpeed()
    {
        if (!swinging)
        {
            if (Mathf.Abs(rb2D.velocity.x) > maxHorizontalSpeed)
            {
                rb2D.velocity = new Vector3(rb2D.velocity.x - (rb2D.velocity.x * Time.deltaTime * 3), rb2D.velocity.y);
            }
        }
    }

    void Jump(bool jump, Vector2 move)
    {
        if (grounded && jump)
        {
            juicyScaling.Boing();

            if (dashing)
            {
                DashJump();
                return;
            }

            if (canBounce)
            {
                BounceJump();
                return;
            }

            RegularJump();
        }
        else
        {
            if (coyote && jump)
            {
                if (dashing)
                {
                    DashJump();
                    return;
                }
                CoyoteJump();
                return;
            }

            if (swingJumpCooldownTimer <= 0)  // Swing jump
            {
                if (wasSwingingTimer >= 0 && !swinging && move.y > 0)
                {
                    SwingJump();
                    return;
                }
            }

            if (canAirJump && jump)
            {
                AirJump();
            }
        }

        if (wallSlideCooldownTimer <= 0)
        {
            if (!grounded && wallSlide && jump) WallJump();
        }
    }

    void DashJump()
    {
        Jump(1.2f);
        poofParticles.Play();
        Overseer.Instance.audioManager.playerAudio.RegularJump();
    }

    void BounceJump()
    {
        Jump(1.2f);
        poofParticles.Play();
        Overseer.Instance.audioManager.playerAudio.RegularJump();
    }

    void RegularJump()
    {
        Jump(1);
        poofParticles.Play();
        Overseer.Instance.audioManager.playerAudio.RegularJump();
    }

    void Jump(float jumpForceMultiplier)
    {
        float force = jumpForce * jumpForceMultiplier;
        grounded = false;
        dashing = false;
        rb2D.AddForce(new Vector2(0f, force));
        SetVariableJumpHeightTimer();
        wallSlideCooldownTimer = wallSlideCooldownTime;
    }

    void CoyoteJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(new Vector2(0f, jumpForce));
        coyote = false;
        SetVariableJumpHeightTimer();

        poofParticles.Play();
    }

    void SwingJump()
    {
        rb2D.AddForce(new Vector2(0f, (jumpForce / 2)));
        wasSwingingTimer = 0;
        swingJumpCooldownTimer = swingJumpCooldownTime;
        SetVariableJumpHeightTimer();

        Overseer.Instance.audioManager.playerAudio.SwingJump();
    }

    void AirJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x * 0.5f, rb2D.velocity.y < 0 ? rb2D.velocity.y * 0.5f : rb2D.velocity.y);
        RegularJump();
        canAirJump = false;
    }

    void WallJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(new Vector2(facingRight ? -jumpForce : jumpForce, jumpForce * 1.3f));
        SetVariableJumpHeightTimer();
        wallSlideCooldownTimer = wallSlideCooldownTime;
        wallSlide = false;

        poofParticles.Play();

        Overseer.Instance.audioManager.playerAudio.WallJump();
    }

    public void EnemyBounce()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(new Vector2(0f, jumpForce));
        coyote = false;
        SetVariableJumpHeightTimer();

        poofParticles.Play();

        Overseer.Instance.audioManager.playerAudio.BounceJump();
    }

    void VaryJumpHeight(bool holdingJump, Vector2 move)
    {
        if (variableJumpHeightTimer >= 0)
        {
            int influence = holdingJump ? 1 : move.y > 0 ? 1 : 0;
            float targetVelocityY = rb2D.velocity.y + (influence  * variableJumpForce * Time.fixedDeltaTime);
            rb2D.velocity = new Vector2(rb2D.velocity.x, targetVelocityY);
        }
    }

    void SetVariableJumpHeightTimer()
    {
        variableJumpHeightTimer = variableJumpHeightTime;
    }

    void FlipPlayer(Vector2 move)
    {
        // If the input is moving the player right and the player is facing left...
        if (move.x > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move.x < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void WallSlide(Vector2 move)
    {
        WallChecks(move);

        if (wallSlide)
        {
            canAirJump = false;
            canDash = true;
            dashing = false;
            if (rb2D.velocity.y < wallSlideSpeed)
            {
                float deceleration = (-rb2D.velocity.y - wallSlideSpeed) * Time.deltaTime * 10;
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y + deceleration);
            }
        }
    }

    Collider2D[] wallColliders;
    void WallChecks(Vector2 move)
    {
        wallSlide = false;
        int noOfWallColliders = Physics2D.OverlapCircleNonAlloc(wallCheck.position, wallCheckRadius, wallColliders, whatIsGround);
        for (int i = 0; i < noOfWallColliders; i++)
        {
            if (wallColliders[i].gameObject != gameObject)
            {
                if (facingRight && move.x > 0 || !facingRight && move.x < 0)
                {
                    wallSlide = true;
                }
            }
        }
    }

    #region Special occasions

    void MovingPlatforms(MovingPlatform platform)
    {
        if (platform is WaterfallPlatform)
        {
            var waterfallPlat = platform as WaterfallPlatform;
            if (platform.DeltaPos().magnitude > (waterfallPlat.distance * 0.9f))
            {
                return;
            }
        }
        if (platform is BreakingPlatform)
        {
            var breakingPlatform = platform as BreakingPlatform;
            breakingPlatform.breaking = true;
        }
        transform.position = transform.position + platform.DeltaPos();
        rb2D.velocity = rb2D.velocity + platform.DeltaVelocity();
    }

    bool erupted;
    const float eruptTime = 0.3f;
    float eruptTimer;
    void EruptTimer()
    {
        if (erupted)
        {
            eruptTimer -= Time.deltaTime;
            if (eruptTimer <= 0)
            {
                erupted = false;
            }
        }
    }

    public void Volcano()
    {
        if (!erupted)
        {
            grounded = false;
            dashing = false;
            canDash = true;
            canAirJump = false;
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(new Vector2(0f, jumpForce * 2.5f));
            SetVariableJumpHeightTimer();
            wallSlideCooldownTimer = wallSlideCooldownTime;

            poofParticles.Play();

            Overseer.Instance.audioManager.playerAudio.RegularJump(); // different audio call at some point??

            erupted = true;
            eruptTimer = eruptTime;
        }
        
    }

    #endregion

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(groundCheck.position, new Vector3(groundCheckBox.x, groundCheckBox.y, 1));
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
#endif
}