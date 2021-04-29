using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrRedMove : MonoBehaviour
{

    private Animator Anim;
    private BoxCollider2D Box;
    public float WalkSpeed = 0.001f;
    public float JumpSpeed = 0.001f;
    private bool IsJumping = false;
    private bool CanMoveLeft = true;
    private bool CanMoveRight = true;
    public GameObject Player;
    public GameObject Opponent;
    private Vector3 OpponentPosition;
    public static bool FaceRight = true;
    public static bool FaceLeft = false;
    public static bool RedWalkLeft = true;
    public static bool RedWalkRight = true;
    private AnimatorStateInfo PlayerLayer;
    private bool Attacking = false;
    public float DamageAmount = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Box = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Screenbounds
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
        if (ScreenBounds.x > Screen.width - 50)
        {
            CanMoveRight = false;
        }
        else if (ScreenBounds.x < 50)
        {
            CanMoveLeft = false;
        }
        else
        {
            CanMoveLeft = true;
            CanMoveRight = true;
        }

        //Get Opponent's position and flip to face them
        OpponentPosition = Opponent.transform.position;
        if(OpponentPosition.x > transform.position.x)
        {
            if (FaceLeft)
            {
                transform.Rotate(0, -180, 0);
                FaceLeft = false;
                FaceRight = true;
            }
        }
        else if (OpponentPosition.x < transform.position.x)
        {
            if (FaceRight)
            {
                transform.Rotate(0, 180, 0);
                FaceRight = false;
                FaceLeft = true;
            }
        }

        //Walking Left and Right

        PlayerLayer = Anim.GetCurrentAnimatorStateInfo(0);

        if (PlayerLayer.IsTag("Motion"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                //Checks for screenbounds
                if (CanMoveRight)
                {
                    //Checks for player collision
                    if (RedWalkRight == true)
                    {
                        //Reverses controls for flipped character
                        if (FaceLeft)
                        {
                            Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                        else
                        {
                            Anim.SetBool("Walk", true);
                            transform.Translate(WalkSpeed, 0, 0);
                        }
                    }
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                //Checks for screenbounds
                if (CanMoveLeft)
                {
                    //Checks for player collision
                    if (RedWalkLeft == true)
                    {
                        //Reverses controls for flipped character
                        if (FaceLeft)
                        {
                            Anim.SetBool("Walk", true);
                            transform.Translate(WalkSpeed, 0, 0);
                        }
                        else
                        {
                            Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                    }
                }
            }
            else
            {
                Anim.SetBool("Walk", false);
            }
        }

        //Jumping and Crouching, checks for blocking
            if (Input.GetAxis("Vertical") > 0)
            {
                if (!IsJumping)
                {
                    Anim.SetTrigger("Jump");
                    IsJumping = true;
                }
                transform.Translate(0, JumpSpeed, 0);
                Anim.SetBool("Grounded", false);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                Anim.SetBool("Crouch", true);
            }
            else
            {
                Anim.SetBool("Crouch", false);
                Anim.SetBool("Grounded", true);
                IsJumping = false;
            }

        //Grounded Attacks
        if (Input.GetButtonDown("Fire1"))
        {
            if (!Attacking)
            {
                Anim.SetTrigger("LightAtk");
                Attacking = true;
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (!Attacking)
            {
                Anim.SetTrigger("HeavyAtk");
                Attacking = true;
            }
        }
        else
        {
            Attacking = false;
        }

        //Blocking
        if (Input.GetButton("Fire3"))
        {
            Anim.SetBool("Block", true);
        }
        if (Input.GetButtonUp("Fire3"))
        {
            Anim.SetBool("Block", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (!Anim.GetBool("Block"))
        {
            if (Other.CompareTag("Damage"))
            {
                Anim.SetTrigger("DamageTaken");
                HealthState.Player1Health -= DamageAmount;
                if (HealthState.Player1Timer < 2.0f)
                {
                    HealthState.Player1Timer += 2.0f;
                }
            }
        }   
    }
}
