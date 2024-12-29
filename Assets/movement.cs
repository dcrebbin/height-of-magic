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
    [SerializeField] private float dashCooldown = 1f;
    private float lastDashTime = 0f;
    public bool isFacingRight = true;

    public Attack attack;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
    }


    void Update()
    {
        if(attack.isDefending){
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.velocity = movement * speed;
        if(moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else if(moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
    }

    void FixedUpdate(){
            if(Input.GetKeyDown(KeyCode.Q)){
                rb.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
                animator.SetTrigger(isFacingRight ? "dashLeft" : "dashRight");
            }else if(Input.GetKeyDown(KeyCode.E)){
                rb.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
                animator.SetTrigger(isFacingRight ? "dashRight" : "dashLeft");
            }
    }

    void LateUpdate()
    {
        animator.SetFloat("moveX", rb.velocity.x);
        animator.SetBool("isMoving", rb.velocity.magnitude > 0.1f);
    }
}
