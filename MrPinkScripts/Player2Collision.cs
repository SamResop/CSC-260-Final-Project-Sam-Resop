using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Collision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player1Move.FaceLeft && collision.gameObject.CompareTag("P1Right"))
        {
            Player2Move.WalkRight = false; 
        }
        if (collision.gameObject.CompareTag("P1Right") && !Player1Move.FaceLeft)
        {
            Player2Move.WalkLeft = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Player1Move.FaceLeft && collision.gameObject.CompareTag("P1Right"))
        {
            Player2Move.WalkRight = true;
        }
        if (collision.gameObject.CompareTag("P1Right"))
        {
            Player2Move.WalkLeft = true;
        }
    }
}
