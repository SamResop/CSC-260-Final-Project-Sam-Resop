using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Collision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player1Move.FaceLeft && collision.gameObject.CompareTag("P2Right"))
        {
            Player1Move.RedWalkLeft = false; 
        }
        if (collision.gameObject.CompareTag("P2Right") && !Player1Move.FaceLeft)
        {
            Player1Move.RedWalkRight = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Player1Move.FaceLeft && collision.gameObject.CompareTag("P2Right"))
        {
            Player1Move.RedWalkLeft = true;
        }
        if (collision.gameObject.CompareTag("P2Right"))
        {
            Player1Move.RedWalkRight = true;
        }
    }
}
