using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    public Camera myCamera;
    public HookScript myHook;
    public float MaxMoveSpeed = 1f;
    float currentMaxMoveSpeed = 1f;
    public float TimeToStop = 1f;

    public float MoveAcceleration = 1f;
    public float GroundedStopSpeed = 1f;
    public float Sensitivity = 2f;
    public GunScript CurrentGun;

    CameraController cameraController;

    public bool Grounded;
    public float JumpForce;
    public float RocketJumpForce;

    public float RisingMass;
    public float FallingMass;

    float horizontal, vertical, mouseX, mouseY;

    bool hook, shoot, jump, rocketJump;

    SphereCollider col;
    public LayerMask groundLayers;

    public ReticleScript reticle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraController = myCamera.GetComponent<CameraController>();
        col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Grounded = IsGrounded();

        input();

        if(Grounded && Mathf.Abs(horizontal) < .1f && Mathf.Abs(vertical) < .1f)
        {
            currentMaxMoveSpeed = Mathf.MoveTowards(currentMaxMoveSpeed, 0, TimeToStop);
        }
        else
        {
            currentMaxMoveSpeed = MaxMoveSpeed;
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = (transform.right * horizontal + transform.forward * vertical) * MoveSpeed + new Vector3(0, rb.velocity.y, 0);

        rb.AddForce((transform.right * horizontal + transform.forward * vertical) * MoveAcceleration, ForceMode.Acceleration);

        if(rb.velocity.sqrMagnitude > currentMaxMoveSpeed * currentMaxMoveSpeed)
        {
            Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = horizontalVel.normalized * currentMaxMoveSpeed + new Vector3(0, rb.velocity.y, 0);
        }

        transform.rotation = Quaternion.Euler(0, myCamera.transform.rotation.eulerAngles.y, 0);

        if (jump && Grounded)
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;
            
            rb.velocity = vel;

            rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            jump = false;
            GetComponent<AudioSource>().Play();
        }

        if(rocketJump)
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;

            rb.velocity = vel;

            rb.AddForce(new Vector3(0, RocketJumpForce, 0), ForceMode.Impulse);
            rocketJump = false;
        }

        //if (rb.velocity.y < 0)
        //{
        //    rb.mass = FallingMass;
        //}
        //else
        //{
        //    rb.mass = RisingMass;
        //}

        cameraController.AddRotation(-mouseY, mouseX, 0, Sensitivity);
        CurrentGun.LeanGunWithAim(mouseX, mouseY, horizontal);
    }

    void input()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        hook = Input.GetButtonDown("Fire2");
        shoot = Input.GetButtonDown("Fire1");

        if(Input.GetButtonDown("Jump") && Grounded)
        {
            jump = true;
        }


        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        reticle.LeanReticle(new Vector2(mouseX, mouseY));

        if (!myHook.isHooking)
        {
            if(hook && myHook.enemyOnHook == false)
            {
                myHook.SendHook();
            }
            else if(shoot)
            {
                if(myHook.enemyOnHook == false)
                {
                    CurrentGun.Shoot(shoot);
                }
                else
                {
                    myHook.ShootEnemyOffOfHook();
                    if(Camera.main.transform.forward.y < -.8f)
                    {
                        rocketJump = true;
                    }
                    //CurrentGun.Shoot(shoot);
                }
            }
            
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y-.5f, col.bounds.center.z), col.radius * .9f, groundLayers);
    }
}
