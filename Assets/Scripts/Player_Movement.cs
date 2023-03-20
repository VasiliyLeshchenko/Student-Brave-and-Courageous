using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private enum MovementState
    {
        idle,
        running,
		jumping,
		falling

    }

    private float moveInput = 0f;
    private float gravityScale = 1.2f;

    [Header("Movement")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float velPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Header("Jump")]

	[SerializeField] private float jumpForce;
	[SerializeField] private float fallGravityMultiplier;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown("space"))
		{
			Jump();
		}

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        //целевая скорость
        float targetSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        // разница между целевой и настоящей скоростью
        float speedDif = targetSpeed - rb.velocity.x;

        // если целевая скорость + то ускоряем, если - то замедляем
        float acceleRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;


        float movement = Mathf.Pow(Mathf.Abs(speedDif) * acceleRate, velPower) * Mathf.Sign(speedDif);


        rb.AddForce(movement * Vector2.right);

    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (moveInput > 0)
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }
}
