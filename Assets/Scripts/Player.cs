using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    //necesarias para las animaciones y fisica
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;

    [Header("Ground Vars")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float radOCircle;
    public bool grounded;

    [Header("Movement Vars")]
    public float speed = 2.0f;
    public float horizMovement;
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stopJump;

    // Start is called before the first frame update
    private void Start()
    {
        //define gameObjects de player
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }

    // Update is called once per frame, handles the inputs
    private void Update()
    {
        //check if the player has input movement
        horizMovement = Input.GetAxisRaw("Horizontal");
        //what it means to be grounded
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOCircle, ground);

        //reset jumpTimeCounter
        if(grounded){
            jumpTimeCounter = jumpTime;
            anim.ResetTrigger("jump");
            anim.SetBool("falling", false);
        }

        //jump button pressed
        if(Input.GetButtonDown("Jump") && grounded){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            stopJump = false;
            anim.SetTrigger("jump");
        }

        //keep jumping while jump button is down
        if(Input.GetButton("Jump") && !stopJump && (jumpTimeCounter > 0)){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
            anim.SetTrigger("jump");
        }

        //jump button release
        if(Input.GetButtonUp("Jump")){
            jumpTimeCounter = 0;
            stopJump = true;
            anim.SetBool("falling", true);
            anim.ResetTrigger("jump");
        }

        if(rb.velocity.y < 0){
            anim.SetBool("falling", true);
        }
    }

    //handles running the physics
    private void FixedUpdate(){
        //move character
        rb.velocity = new Vector2(horizMovement * speed, rb.velocity.y);
        Flip(horizMovement);
        anim.SetFloat("speed", Mathf.Abs(horizMovement));
        HandleLayers();
    }

    private void Flip(float horizontal){
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight){
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawSphere(groundCheck.position, radOCircle);
    }

    private void HandleLayers(){
        if(!grounded){
            anim.SetLayerWeight(1, 1);
        }else{
            anim.SetLayerWeight(1, 0);
        }
    }
}
