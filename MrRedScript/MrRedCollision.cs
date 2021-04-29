using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrRedCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MrRedMove.FaceLeft && collision.gameObject.CompareTag("P2Right"))
        {
            MrRedMove.RedWalkLeft = false; 
        }
        if (collision.gameObject.CompareTag("P2Right") && !MrRedMove.FaceLeft)
        {
            MrRedMove.RedWalkRight = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MrRedMove.FaceLeft && collision.gameObject.CompareTag("P2Right"))
        {
            MrRedMove.RedWalkLeft = true;
        }
        if (collision.gameObject.CompareTag("P2Right"))
        {
            MrRedMove.RedWalkRight = true;
        }
    }
}
