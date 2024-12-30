using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 4f;
    private float lastDashTime = 0f;
    [SerializeField] private float stepDelay = 0.2f;
    private Coroutine stepCoroutine;
    private SoundManager soundManager;
    public bool isFacingRight = true;

    public Attack attack;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;

    public bool isSleeping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(nameof(stepping));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSleeping = !isSleeping;
        }

        if (isSleeping)
        {
            return;
        }

        if (attack.isDefending)
        {
            return;
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
            return; // Skip regular movement while dashing
        }



        // Handle dash input
        if (Time.time >= lastDashTime + dashCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartDash(-1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartDash(1);
            }
        }

        // Regular movement (only if not dashing)
        if (!isDashing)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(moveX, moveY).normalized;
            if (attack.isCharging)
            {
                rb.velocity = movement * speed * 0.3f;
            }
            else
            {
                rb.velocity = movement * speed;
            }

            // Handle facing direction
            if (moveX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingRight = false;
            }
            else if (moveX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingRight = true;
            }
        }
    }

    IEnumerator stepping()
    {
        while (enabled)
        {
            if (rb.velocity.magnitude > 0.1f)
            {

                soundManager.playStepSound();
                yield return new WaitForSeconds(stepDelay);
            }

            yield return null;
        }
    }


    private void StartDash(float direction)
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(dashSpeed * direction, 0), ForceMode2D.Impulse);
        animator.SetTrigger(direction > 0 ?
            (isFacingRight ? "dashRight" : "dashLeft") :
            (isFacingRight ? "dashLeft" : "dashRight"));
    }

    void LateUpdate()
    {
        animator.SetFloat("moveX", rb.velocity.x);
        animator.SetBool("isMoving", rb.velocity.magnitude > 0.1f);
        animator.SetBool("isSleeping", isSleeping);
    }
}
