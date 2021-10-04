using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Move : MonoBehaviour
{

    public static Animator P2Anim;
    private BoxCollider2D Box;
    public float WalkSpeed = 7.00f;
    public float JumpSpeed = 7.00f;
    private bool IsJumping = false;
    private bool CanMoveLeft = true;
    private bool CanMoveRight = true;
    private bool crouch = false;
    public GameObject Player;
    public GameObject Opponent;
    public Vector3 OpponentPosition;
    private bool FaceRight = true;
    private bool FaceLeft = false;
    public static bool WalkLeft = true;
    public static bool WalkRight = true;
    private AnimatorStateInfo PlayerLayer;
    private bool Attacking = false;
    public static float DamageTakenP2 = 0.1f;
    public GameObject Projectile2;
    public static bool projectileAliveP2 = false;


    // Start is called before the first frame update
    void Start()
    {
        Opponent = GameObject.Find("Player1");
        P2Anim = GetComponent<Animator>();
        Box = GetComponentInChildren<BoxCollider2D>();
        CanMoveLeft = true;
        CanMoveRight = true;
        FaceRight = true;
        FaceLeft = false;
        WalkLeft = true;
        WalkRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for win or lose
        if(HealthState.Player2Health <= 0)
        {
            P2Anim.SetTrigger("Death");
            //this.GetComponent<MrPinkMove>().enabled = false;
            StartCoroutine(Dead());
        }
        else if(HealthState.Player1Health <=0)
        {
            P2Anim.SetTrigger("Victory");
            StartCoroutine(Won());
            //this.GetComponent<MrPinkMove>().enabled = false;
        }
 
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

        PlayerLayer = P2Anim.GetCurrentAnimatorStateInfo(0);

        if (PlayerLayer.IsTag("Motion"))
        {
            if (Input.GetAxis("HorizontalP2") > 0)
            {
                //Checks for screenbounds
                if (CanMoveRight)
                {
                    //Checks for player collision
                    if (WalkRight == true)
                    {
                        //Reverses controls for flipped character
                        if (FaceLeft)
                        {
                            P2Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                        else
                        {
                            P2Anim.SetBool("Walk", true);
                            transform.Translate(WalkSpeed, 0, 0);
                        }
                    }
                }
            }
            else if (Input.GetAxis("HorizontalP2") < 0)
            {
                //Checks for screenbounds
                if (CanMoveLeft)
                {
                    //Checks for player collision
                    if (WalkLeft == true)
                    {
                        //Reverses controls for flipped character
                        if (FaceLeft)
                        {
                            P2Anim.SetBool("Walk", true);
                            transform.Translate(WalkSpeed, 0, 0);
                        }
                        else
                        {
                            P2Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                    }
                }
            }
            else
            {
                P2Anim.SetBool("Walk", false);
            }
        }

        //Jumping and Crouching, checks for blocking
            if (Input.GetAxis("VerticalP2") > 0)
            {
                if (!IsJumping)
                {
                    P2Anim.SetTrigger("Jump");
                    IsJumping = true;
                }
                transform.Translate(0, JumpSpeed, 0);
            
            }
            else if (Input.GetAxis("VerticalP2") < 0)
            {
                P2Anim.SetBool("Crouch", true);
                crouch = true;
            }
            else
            {
                P2Anim.SetBool("Crouch", false);
                P2Anim.SetBool("Grounded", true);
                IsJumping = false;
                crouch = false;
            }
            if (transform.position.y > -1)
            {
                P2Anim.SetBool("Grounded", false);
            }
        //Grounded Attacks
        if (Input.GetButtonDown("LightAtkP2"))
        {
            if (!Attacking)
            {
                Player1Move.DamageTakenP1 = 0.05f;
                P2Anim.SetTrigger("LightAtk");
                Attacking = true;
            }
        }
        else if (Input.GetButtonDown("HeavyAtkP2"))
        {
            if (!Attacking)
            {
                Player1Move.DamageTakenP1 = 0.1f;
                P2Anim.SetTrigger("HeavyAtk");
                Attacking = true;
            }
        }
        //Projectile
        else if (Input.GetButtonDown("ProjectileP2") && !crouch)
        {
            P2Anim.SetTrigger("Shoot");
            Player1Move.DamageTakenP1 = 0.05f;
            StartCoroutine(Projectile());
            Attacking = true;
        }
        else
        {
            Attacking = false;
        }

        //Blocking
        if (Input.GetButton("BlockP2"))
        {
            P2Anim.SetBool("Block", true);
        }
        if (Input.GetButtonUp("BlockP2"))
        {
            P2Anim.SetBool("Block", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Player1") && transform.position.y > 0)
            transform.Translate(-0.5f, 0, 0);

        //Check for lows
        if (!P2Anim.GetBool("Block"))
            {
                if (Other.CompareTag("Damage"))
                {
                    //P2Anim.SetTrigger("DamageTaken");
                    //for (int i = 0; i < 10; i++)
                    //    transform.Translate(-0.1f, 0, 0);
                    StartCoroutine(Hitstun());
                    HealthState.Player2Health -= DamageTakenP2;
                    if (HealthState.Player2Timer < 2.0f)
                    {
                        HealthState.Player2Timer += 2.0f;
                    }
                }
                else if (Other.CompareTag("ProjectileP1"))
                {
                    StartCoroutine(Hitstun());
                    HealthState.Player2Health -= DamageTakenP2;
                    if (HealthState.Player2Timer < 2.0f)
                    {
                        HealthState.Player2Timer += 2.0f;
                    }
                    Destroy(Other.gameObject);
                    Player1Move.projectileAliveP1 = false;
                }
            }
            //Chip Damage
            else
            {
                if (Other.CompareTag("Damage") || Other.CompareTag("ProjectileP1"))
                {
                    if (Other.CompareTag("ProjectileP1"))
                    {
                        Destroy(Other.gameObject);
                        Player1Move.projectileAliveP1 = false;
                    }
                    //Check for lows
                    if (Player1Move.P1Anim.GetBool("Crouch") && !P2Anim.GetBool("Crouch") && !Other.CompareTag("ProjectileP1"))
                    {
                        HealthState.Player2Health -= DamageTakenP2;
                        if (HealthState.Player2Timer < 2.0f)
                        {
                            HealthState.Player2Timer += 2.0f;
                        }
                        StartCoroutine(Hitstun());
                    }
                    else if (!Player1Move.P1Anim.GetBool("Grounded") && P2Anim.GetBool("Crouch") && !Other.CompareTag("ProjectileP1"))
                    {
                        HealthState.Player2Health -= DamageTakenP2;
                        if (HealthState.Player2Timer < 2.0f)
                        {
                            HealthState.Player2Timer += 2.0f;
                        }
                        StartCoroutine(Hitstun());
                    }
                    else
                    {
                        DamageTakenP2 = 0.01f;
                        HealthState.Player2Health -= DamageTakenP2;
                        if (HealthState.Player2Timer < 2.0f)
                        {
                            HealthState.Player2Timer += 2.0f;
                        }
                    }
                }
            }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Player2Move>().enabled = false;
        P2Anim.enabled = false;
    }

    IEnumerator Won()
    {
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Player2Move>().enabled = false;
        P2Anim.enabled = false;
    }

    IEnumerator Hitstun()
    {
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
        this.GetComponent<Player2Move>().enabled = false;
        P2Anim.SetTrigger("DamageTaken");
        for (int i = 0; i < 10; i++)
        {
            ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
            if (ScreenBounds.x < Screen.width - 50 && ScreenBounds.x > 50)
                transform.Translate(-0.15f, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
        this.GetComponent<Player2Move>().enabled = true;
    }

    IEnumerator Projectile()
    {
        if (!projectileAliveP2)
        {
            projectileAliveP2 = true;
            GameObject blast;
            yield return new WaitForSeconds(.5f);
            blast = Instantiate(Projectile2, transform.position, transform.rotation);
            blast.transform.Translate(2, 1, 0);
            for (int i = 0; i < 50; i++)
            {
                if (blast != null)
                {
                    blast.transform.Translate(0.15f, 0, 0);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            if (blast != null)
                Destroy(blast);
            projectileAliveP2 = false;
        }
    }
}
