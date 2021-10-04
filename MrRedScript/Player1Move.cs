using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1Move : MonoBehaviour
{

    public static Animator P1Anim;
    private BoxCollider2D Box;
    public float WalkSpeed = 5.0f;
    public float JumpSpeed = 5.0f;
    private bool IsJumping = false;
    private bool CanMoveLeft = true;
    private bool CanMoveRight = true;
    private bool crouch = false;
    public GameObject Player;
    public GameObject Opponent;
    private Vector3 OpponentPosition;
    public static bool FaceRight = true;
    public static bool FaceLeft = false;
    public static bool RedWalkLeft = true;
    public static bool RedWalkRight = true;
    private AnimatorStateInfo PlayerLayer;
    private bool Attacking = false;
    public static float DamageTakenP1 = 0.1f;
    public GameObject projectile;
    public static bool projectileAliveP1 = false;


    // Start is called before the first frame update
    void Start()
    {
        Opponent = GameObject.Find("Player2");
        P1Anim = GetComponent<Animator>();
        Box = GetComponentInChildren<BoxCollider2D>();
        FaceRight = true;
        FaceLeft = false;
        RedWalkLeft = true;
        RedWalkRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for win or lose
        if (HealthState.Player1Health <= 0)
        {
            P1Anim.SetTrigger("Death");
            //this.GetComponent<MrRedMove>().enabled = false;
            StartCoroutine(Dead());
        }
        else if (HealthState.Player2Health <= 0)
        {
            P1Anim.SetTrigger("Victory");
            StartCoroutine(Won());
            //this.GetComponent<MrRedMove>().enabled = false;
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

        PlayerLayer = P1Anim.GetCurrentAnimatorStateInfo(0);

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
                            P1Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                        else
                        {
                            P1Anim.SetBool("Walk", true);
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
                            P1Anim.SetBool("Walk", true);
                            transform.Translate(WalkSpeed, 0, 0);
                        }
                        else
                        {
                            P1Anim.SetBool("Walk", true);
                            transform.Translate(-WalkSpeed, 0, 0);
                        }
                    }
                }
            }
            else
            {
                P1Anim.SetBool("Walk", false);
            }
        }

        //Jumping and Crouching, checks for blocking
            if (Input.GetAxis("Vertical") > 0)
            {
                if (!IsJumping)
                {
                    P1Anim.SetTrigger("Jump");
                    IsJumping = true;
                }
                transform.Translate(0, JumpSpeed, 0);
                //P1Anim.SetBool("Grounded", false);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                P1Anim.SetBool("Crouch", true);
                P1Anim.SetBool("Grounded", true);
                crouch = true;
            }
            else
            {
                P1Anim.SetBool("Crouch", false);
                P1Anim.SetBool("Grounded", true);
                IsJumping = false;
                crouch = false;
            }
            if (transform.position.y > -1)
            {
                P1Anim.SetBool("Grounded", false);
            }

        //Grounded Attacks
        if (Input.GetButtonDown("Fire1"))
        {
            if (!Attacking)
            {
                Player2Move.DamageTakenP2 = 0.05f;
                P1Anim.SetTrigger("LightAtk");
                Attacking = true;
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (!Attacking)
            {
                Player2Move.DamageTakenP2 = 0.1f;
                P1Anim.SetTrigger("HeavyAtk");
                Attacking = true;
            }
        }
        //Projectile
        else if (Input.GetButtonDown("Fire3") && !crouch)
        {
            P1Anim.SetTrigger("Shoot");
            Player2Move.DamageTakenP2 = 0.05f;
            StartCoroutine(Projectile());
            Attacking = true;
        }
        else
        {
            Attacking = false;
        }

        //Blocking
        if (Input.GetButton("Fire4"))
        {
            P1Anim.SetBool("Block", true);
        }
        if (Input.GetButtonUp("Fire4"))
        {
            P1Anim.SetBool("Block", false);
        }
    }

    //Taking Damage
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Player2") && transform.position.y > 0)
            transform.Translate(-0.5f, 0, 0);

        if (!P1Anim.GetBool("Block"))
        {
            if (Other.CompareTag("Damage"))
            {
                StartCoroutine(Hitstun());
                P1Anim.SetTrigger("DamageTaken");
                HealthState.Player1Health -= DamageTakenP1;
                if (HealthState.Player1Timer < 2.0f)
                {
                    HealthState.Player1Timer += 2.0f;
                }
            }
            else if (Other.CompareTag("ProjectileP2"))
            {
                StartCoroutine(Hitstun());
                HealthState.Player1Health -= DamageTakenP1;
                if (HealthState.Player1Timer < 2.0f)
                {
                    HealthState.Player1Timer += 2.0f;
                }
                Destroy(Other.gameObject);
                Player2Move.projectileAliveP2 = false;
            }
        }
        //Chip Damage
        else
        {
            if (Other.CompareTag("Damage") || Other.CompareTag("ProjectileP2"))
            {
                if (Other.CompareTag("ProjectileP2"))
                {
                    Destroy(Other.gameObject);
                    Player2Move.projectileAliveP2 = false;
                }
                //Check for lows
                if (Player2Move.P2Anim.GetBool("Crouch") && !P1Anim.GetBool("Crouch") && !Other.CompareTag("ProjectileP2"))
                {
                    HealthState.Player1Health -= DamageTakenP1;
                    if (HealthState.Player1Timer < 2.0f)
                    {
                        HealthState.Player1Timer += 2.0f;
                    }
                    StartCoroutine(Hitstun());
                }
                else if (!Player2Move.P2Anim.GetBool("Grounded") && P1Anim.GetBool("Crouch") && !Other.CompareTag("ProjectileP2"))
                {
                    HealthState.Player1Health -= DamageTakenP1;
                    if (HealthState.Player1Timer < 2.0f)
                    {
                        HealthState.Player1Timer += 2.0f;
                    }
                    StartCoroutine(Hitstun());
                }
                else
                {
                    DamageTakenP1 = 0.01f;
                    HealthState.Player1Health -= DamageTakenP1;
                    if (HealthState.Player1Timer < 2.0f)
                    {
                        HealthState.Player1Timer += 2.0f;
                    }

                }
            }
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Player1Move>().enabled = false;
        P1Anim.enabled = false;
        SceneManager.LoadScene(0);
    }

    IEnumerator Won()
    {
        yield return new WaitForSeconds(1.5f);
        this.GetComponent<Player1Move>().enabled = false;
        SceneManager.LoadScene(0);
    }
    
    IEnumerator Hitstun()
    {
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
        this.GetComponent<Player1Move>().enabled = false;
        P1Anim.SetTrigger("DamageTaken");
        for (int i = 0; i < 10; i++)
        {
            ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
            if (ScreenBounds.x < Screen.width - 50 && ScreenBounds.x > 50)
                transform.Translate(-0.15f, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
        this.GetComponent<Player1Move>().enabled = true;
    }

    IEnumerator Projectile()
    {
        if (!projectileAliveP1)
        {
            projectileAliveP1 = true;
            GameObject blast;
            yield return new WaitForSeconds(.5f);
            blast = Instantiate(projectile, transform.position, transform.rotation);
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
            projectileAliveP1 = false;
        }
    }
}