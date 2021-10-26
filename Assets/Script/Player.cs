using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class Player : MonoBehaviour
{

    public LayerMask ground, anvil;
    bool grounded;
    public float topSpeed, speed, jump, minimumJump, jumpForce, dash, crouchHeadroom;
    public GameObject ragdoll, explosion;
    Rigidbody2D rb;
    BoxCollider2D bc;
    CircleCollider2D cc;
    PolygonCollider2D pc;
    Animator anim;
    bool walk = true; //ability to walk 
    bool canDive = true;
    int framesSinceLastTouch = 0;
    public bool dead = false;
    bool crouching = false;
    bool leftSide, rightSide;

    public bool invincible = false;

    bool groundedCooldown = false; //Cooldown for being able to be grounded again.

    bool airDashRestored = false;

    bool jumpFramesListening = false;
    int jumpFramesListened = 0;
    public int maxJumpFrames = 5;

    public AudioClip left_foot_step, right_foot_step, jump_sfx, land;

    public PhysicsMaterial2D sliding, idle, frictionless;

    bool leftSideGrounded, rightSideGrounded = false;

    AudioSource aus;

    int fixedFramesSinceLastGround = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        cc = GetComponent<CircleCollider2D>();
        pc = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        aus = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            if (rightSide && ((rightSideGrounded ^ leftSideGrounded) || !grounded))
            {
                WallJump(-1);
            }
            else if (leftSide && ((rightSideGrounded ^ leftSideGrounded) || !grounded))
            {
                WallJump(1);

            }
            else if (fixedFramesSinceLastGround < 10)
            {
                StartCoroutine(CooldownGroundedState());

                // Reset our velocity for consistent jumps
                rb.velocity = new Vector2(rb.velocity.x, minimumJump);

                // Begin listening for jump frames
                jumpFramesListening = true;

                // Reset our older listened frames
                jumpFramesListened = 0;

                anim.SetTrigger("jump");
            }


        // Diving mechanics should always respond on any frame!
        if (Input.GetAxis("Horizontal") != 0 && Input.GetKeyDown(KeyCode.Space) && grounded)
            Dash((int)(Mathf.Abs(Input.GetAxis("Horizontal")) / Input.GetAxis("Horizontal")));
        else if (Input.GetAxis("Horizontal") != 0 && Input.GetKeyDown(KeyCode.Space) && !grounded)
            Dive((int)(Mathf.Abs(Input.GetAxis("Horizontal")) / Input.GetAxis("Horizontal")));

        if(touchedHazardThisFrame && !touchedGroundThisFrame)
        {
            Die();
        }

        touchedGroundThisFrame = false;
        touchedHazardThisFrame = false;
    }

    void FixedUpdate()
    {


        if (jumpFramesListening)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (jumpFramesListened < maxJumpFrames)
                {
                    rb.AddForce(new Vector2(0, jumpForce));
                    jumpFramesListened++;
                }
            }
            else
            {
                // we lifted up W and now we can't jump anymore until we ground again.
                jumpFramesListening = false;
            }
        }

        fixedFramesSinceLastGround++;

        bool previous_grounded = grounded;

        // Split grounded up for better wall jump calculations.
        leftSideGrounded = Physics2D.Raycast(new Vector2(transform.position.x - 0.1f, transform.position.y - .75f), Vector2.down, .2f, ground).collider != null;
        rightSideGrounded = Physics2D.Raycast(new Vector2(transform.position.x + 0.1f, transform.position.y - .75f), Vector2.down, .2f, ground).collider != null;

        if (leftSideGrounded && rightSideGrounded) airDashRestored = true; // if we touch both feet to the ground we get an air dash back

        if (!groundedCooldown)
        {
            grounded = leftSideGrounded || rightSideGrounded;
        }

        if (previous_grounded == false && grounded == true)
        {
            PlaySound("land");
        }

        rightSide = Physics2D.Raycast(new Vector2(transform.position.x + .5f, transform.position.y), Vector2.down, .1f, ground).collider != null;
        leftSide = Physics2D.Raycast(new Vector2(transform.position.x - .5f, transform.position.y), Vector2.down, .1f, ground).collider != null;

        anim.SetBool("grounded", grounded);

        if (grounded)
            fixedFramesSinceLastGround = 0;

        if (Input.GetKeyDown(KeyCode.U))
            Camera.main.gameObject.GetComponent<ProCamera2DCinematics>().Play();

        Crouch(Input.GetKey(KeyCode.S) && grounded);

        // We CAN walk
        if (walk)
        {


            if (Input.GetAxis("Horizontal") != 0)
                anim.SetBool("walking", true);
            else
                anim.SetBool("walking", false);

            // If we give input go fricitonless. If were in the air stay frictionless for walls.
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                cc.sharedMaterial = frictionless;
            else
                if(rightSideGrounded && leftSideGrounded)
                    cc.sharedMaterial = idle;
                else
                    cc.sharedMaterial = frictionless;


            if (Input.GetAxis("Horizontal") < 0 && rb.velocity.x < 0 && grounded)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Input.GetAxis("Horizontal") > 0 && rb.velocity.x > 0 && grounded)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }


            if ((rb.velocity.x < 0 && Input.GetAxis("Horizontal") > 0) || (rb.velocity.x > 0 && Input.GetAxis("Horizontal") < 0))
            {
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed * 5, 0));
            }
            else if (Mathf.Abs(rb.velocity.x) < topSpeed)
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0));

            if (Input.GetKey(KeyCode.S) && !grounded)
                rb.gravityScale = 4;
            else
                rb.gravityScale = 2f;
        }





        /*	Displays an indicator for critical walljump
		 * 
		if ((leftSide||rightSide)&&framesSinceLastTouch < 1)
			transform.Find ("bag").gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
		else
			transform.Find("bag").gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		*/
        framesSinceLastTouch++;

        // Set our walking animation speed based on our x velocity
        anim.SetFloat("walkAnimationMultiplier", 0.5f + (Mathf.Abs(rb.velocity.x) / topSpeed));
    }

    void Dash(int dir)
    {
        if (!canDive) return;

        //StartCoroutine(PlaySmokeTrail());
        StartCoroutine(DisableDive(0.5f));
        StartCoroutine(DisableWalk(0.5f));
        anim.SetTrigger("dash");

        PlaySound("jump");

        rb.velocity = new Vector2(dash * dir, 4);

        StartCoroutine(CooldownGroundedState());

        /*
		if(Mathf.Abs(rb.velocity.x)/rb.velocity.x!=dir)
			rb.velocity=new Vector2(dash*dir,4);
		else if(Mathf.Abs(rb.velocity.x)>topSpeed-dash)
			rb.velocity=new Vector2(rb.velocity.x,4);
		else
			rb.velocity=new Vector2(rb.velocity.x+dash*dir,4);
		*/
    }

    void Dive(int dir)
    {
        if (!airDashRestored) return;
        if (!canDive) return;
        //StartCoroutine(PlaySmokeTrail ());
        StartCoroutine(DisableDive(0.5f));
        StartCoroutine(DisableWalk(0.5f));
        anim.SetTrigger("dash");
        rb.velocity = new Vector2(dash * dir, -4);

        PlaySound("jump");

        airDashRestored = false;



        /*
		if(Mathf.Abs(rb.velocity.x)/rb.velocity.x!=dir)
			rb.velocity=new Vector2(dash*dir,1);
		else if(Mathf.Abs(rb.velocity.x)>topSpeed-dash)
			rb.velocity=new Vector2(rb.velocity.x,1);
		else
			rb.velocity=new Vector2(rb.velocity.x+dash*dir,1);
		*/
    }

    void WallJump(int dir)
    {
        transform.localScale = new Vector3(dir, 1, 1);
        if (framesSinceLastTouch > 1)
        { //normal jump
            rb.velocity = new Vector2(7 * dir, minimumJump);
        }
        else
        { //critical frame perfect <---- this is kinda dumb so im cutting it for now
            rb.velocity = new Vector2(7 * dir, minimumJump);

            //particle and sound
        }

        jumpFramesListening = true;
        jumpFramesListened = 0;

        PlaySound("jump");

        StartCoroutine(DisableWalk(0.45f));
        //StartCoroutine(DisableDive(1f));
    }

    int disableWalkBuffer = 0;

    IEnumerator DisableWalk(float time)
    {
        disableWalkBuffer++;

        walk = false;
        yield return new WaitForSeconds(time);

        disableWalkBuffer--;

        if(disableWalkBuffer == 0)
            walk = true;

    }

    IEnumerator DisableDive(float time)
    {
        canDive = false;
        yield return new WaitForSeconds(time);
        canDive = true;
    }

    IEnumerator ResetCollider(float time)
    {
        yield return new WaitForSeconds(time);
        bc.enabled = true;
    }

    void Crouch(bool c)
    {



        if (c)
        {
            anim.SetBool("crouching", true);
            walk = false;
            crouching = true;
            bc.enabled = false;
            //StartCoroutine(ResetCollider(0));

        }
        else if (crouching && !c &&
            // Nothing above our heads
            Physics2D.Raycast(new Vector2(transform.position.x - 0.1f, transform.position.y ), Vector2.up, crouchHeadroom, ground).collider == null &&
            Physics2D.Raycast(new Vector2(transform.position.x - 0.1f, transform.position.y ), Vector2.up, crouchHeadroom, ground).collider == null
            )
        {
            print("uncrouching");

            anim.SetBool("crouching", false);
            walk = true;
            bc.enabled = true;
            StartCoroutine(ResetCollider(0.15f));
            crouching = false;
        }
    }

    public void Die()
    {
        if (dead || invincible)
            return;

        GameObject rd = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);

        rd.transform.Find("bag").gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-5, 5), Random.Range(-5, 5))).normalized * 7;
        rd.transform.Find("limb1").gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-5, 5), Random.Range(-5, 5))).normalized * 7;
        rd.transform.Find("limb0").gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-5, 5), Random.Range(-5, 5))).normalized * 7;
        rd.transform.Find("bag").gameObject.GetComponent<Rigidbody2D>().angularVelocity = 500;
        rd.transform.Find("limb1").gameObject.GetComponent<Rigidbody2D>().angularVelocity = 500;
        rd.transform.Find("limb0").gameObject.GetComponent<Rigidbody2D>().angularVelocity = 500;

        //SLOW MOTION
        //Time.timeScale = 0.25f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;

        //Camera.main.GetComponent<ProCamera2DShake> ().Shake (0);
        GameManager.gameManager.PlayerDied();

        gameObject.SetActive(false);
        Camera.main.GetComponent<ProCamera2D>().AddCameraTarget(rd.transform.Find("bag"));
        //Destroy (gameObject);
        dead = true;
    }

    bool touchedGroundThisFrame = false;
    bool touchedHazardThisFrame = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        framesSinceLastTouch = 0;

        if (col.gameObject.tag == "Enemy" && col.gameObject.tag != "Passive")
        {
            touchedHazardThisFrame = true;
        }

        if (col.gameObject.layer == 8)
        {
            touchedGroundThisFrame = true;
        }

    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && col.gameObject.tag != "Passive")
        {
            touchedHazardThisFrame = true;
        }
    }


    IEnumerator PlaySmokeTrail()
    {
        transform.Find("smokeTrail").GetComponent<ParticleSystem>().Play();
        transform.Find("smokeTrail").GetComponent<TrailRenderer>().time = 0.5f;
        yield return new WaitForSeconds(0.5f);
        transform.Find("smokeTrail").GetComponent<TrailRenderer>().time = 0;
    }

    IEnumerator CooldownGroundedState()
    {
        grounded = false;

        groundedCooldown = true;
        yield return new WaitForSeconds(0.1f);
        groundedCooldown = false;
    }

    public void PlaySound(string sfx)
    {
        if (sfx == "right_foot_step")
        {
            aus.PlayOneShot(right_foot_step);
        }
        else if (sfx == "left_foot_step")
        {
            aus.PlayOneShot(left_foot_step);
        }
        else if (sfx == "jump")
        {
            aus.PlayOneShot(jump_sfx);
        }
        else if (sfx == "land")
        {
            aus.PlayOneShot(land);
        }
    }
}
