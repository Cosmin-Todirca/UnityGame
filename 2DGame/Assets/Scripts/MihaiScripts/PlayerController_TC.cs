using UnityEngine;

public class PlayerController_TC : KinematicObject_TC
{
    //cosminelul's
    private float velocityMagnitudeError = 0.1f;
    private float velocityYError = 0.1f;
    private float velocityYFallingError = -0.1f;
    public float knockbackForce = 20f;
    public float knockbackForceUp = 0.5f;
    private float knockbackTimerStop = 1.5f;
    private float currentTime = 0f;
    private bool TimerStarter = false;
    Rigidbody2D rigidbody2D;
    //cosminelul's

    public AudioClip jumpAudio;
    public AudioClip respawnAudio;
    public AudioClip ouchAudio;

    public float maxSpeed = 7;
    public PlayerState playerState = PlayerState.Idle;
    public float jumpTakeOffSpeed = 7;

    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;
    public Collider2D collider2d;
    public AudioSource audioSource;
    public Health_TC health;
    public bool controlEnabled = true;

    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;
    internal Animator animator;
    readonly PlatformerModel_TC model = new PlatformerModel_TC();

    public Bounds Bounds => collider2d.bounds;

    //Constante pentru realizarea unui flip decent
    private bool isFacingRight = true;
    private float flipConstant = 5.0f;

    void Awake()
    {
        health = GetComponent<Health_TC>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();              //~~
    }

    protected override void Update()
    {
        if (controlEnabled)
        {
            move.x = Input.GetAxis("Horizontal");
            if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
            {
                jumpState = JumpState.PrepareToJump;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                stopJump = true;

            }
        }
        else
        {
            move.x = 0;
        }
        UpdateJumpState();
        UpdateAnimator();

        //Cu cat player-ul are viata mai mica cu atata devine mai transparent
        Color playerColor = spriteRenderer.color;
        playerColor.a = Mathf.Clamp((float)health.currentHP / health.maxHP, 0.4f, 1);
        spriteRenderer.color = playerColor;

        //cosminelul's
        //control knockbackForce
        if (TimerStarter)
        {
            currentTime += 1 * Time.deltaTime;
            rigidbody2D.velocity = rigidbody2D.velocity / 1.02f;
            if (currentTime > knockbackTimerStop)
            {
                TimerStarter = false;
                rigidbody2D.velocity = new Vector2(0, 0);
                currentTime = 0f;
            }
        }
        //cosminelul's respawn;
        if (health.currentHP == 0)
            health.Respawn();

        base.Update();
    }

    void UpdateAnimator()
    {
        if (playerState == PlayerState.Active)
        {

            if (jumpState == JumpState.Grounded)
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);

            }
            animator.SetFloat("velocity", velocity.x);
        }


    }
    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {

                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {

                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            if (velocity.y > velocityYError)//cosmin's og era 0
            {
                velocity.y = velocity.y * model.jumpDeceleration;
            }
        }

        if (move.x > 0.01f && !isFacingRight)
        {
            Flip();
        }
        else if (move.x < -0.01f && isFacingRight)
        {
            Flip();
        }

        if (velocity.magnitude > velocityMagnitudeError)//cosmin's. OG era 0
        {
            playerState = PlayerState.Active;
            animator.SetBool("Idle", false);
        }
        else
        {
            playerState = PlayerState.Idle;
            animator.SetBool("Idle", true);
        }

        if (velocity.y < velocityYFallingError)//cosmin's og era 0
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }


        animator.SetBool("Grounded", IsGrounded);

        targetVelocity = move * maxSpeed;

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScalePlayer = transform.localScale;
        Vector3 positionPlayer = transform.position;

        localScalePlayer.x *= -1;
        transform.localScale = localScalePlayer;

        //repozitionam player-ul pentru un flip corect
        positionPlayer.x += -localScalePlayer.x * flipConstant;



        transform.position = positionPlayer;



    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    public enum PlayerState
    {
        Idle,
        Active
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("BossDeathZone"))
        {
            health.Die();
        }
        else if (collision.tag.Equals("BossDangerZone"))
        {
            Debug.Log("Player-ul a intrat in DangerZone!");
            health.Decrement(5);
        }
        else if (collision.tag.Equals("Poop"))
        {
            DamageIntake2(collision, 1);
        }
        else if (collision.tag.Equals("Seeker"))
        {
            DamageIntake2(collision, 10);
        }
    }

    //cosminelul's
    /// <summary>
    /// la knockback, daca e lovit din dosul capului, se deplaseaza spre obiectul cu pricina
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blob"))
        {
            DamageIntake(collision, 5);
        }
        else if (collision.gameObject.CompareTag("Seeker"))
        {
            DamageIntake(collision, 10);
        }
    }

    private void DamageIntake(Collision2D collision, int Damage)
    {
        health.Decrement(Damage);
        Vector2 knockbackDirection = new Vector2(transform.position.x - collision.gameObject.transform.position.x, 0); //fiindca ne intereseaza doar daca e din dreapta sau stanga
        rigidbody2D.velocity = new Vector2(knockbackDirection.x, knockbackForceUp) * knockbackForce;
        TimerStarter = true;                    //ca sa oprim velocitatea dupa jumatate de secunda, folosim un timer.
    }
    private void DamageIntake2(Collider2D collision, int Damage)
    {
        health.Decrement(Damage);
        Vector2 knockbackDirection = new Vector2(transform.position.x - collision.gameObject.transform.position.x, 0); //fiindca ne intereseaza doar daca e din dreapta sau stanga
        rigidbody2D.velocity = new Vector2(knockbackDirection.x, knockbackForceUp) * knockbackForce;
        TimerStarter = true;                    //ca sa oprim velocitatea dupa jumatate de secunda, folosim un timer.
    }
}
