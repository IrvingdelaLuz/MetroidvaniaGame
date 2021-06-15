using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : Character
{
    private float runSpeed = 2.0f;
    private float walkSpeed = 1.0f;
    
    public override void Start()
    {
        base.Start();
        speed = runSpeed;
    }

    public override void Update()
    {
        base.Update();
        direction = Input.GetAxisRaw("Horizontal");
        HandleJump();
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();
        anim.SetFloat("speed", Mathf.Abs(direction));
        TunrAround(direction);
    }

    protected override void HandleJump()
    {
        if(grounded){
            jumpTimeCounter = jumpTime;
            anim.ResetTrigger("jump");
            anim.SetBool("falling", false);
        }

        //jump button pressed
        if(Input.GetButtonDown("Jump") && grounded){
            Jump();
            stopJump = false;
            anim.SetTrigger("jump");
        }

        //keep jumping while jump button is down
        if(Input.GetButton("Jump") && !stopJump && (jumpTimeCounter > 0)){
            Jump();
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
    }
}
