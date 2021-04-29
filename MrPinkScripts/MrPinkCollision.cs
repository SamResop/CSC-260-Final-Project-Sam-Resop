using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrPinkCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MrRedMove.FaceLeft && collision.gameObject.CompareTag("P1Right"))
        {
            MrPinkMove.WalkRight = false; 
        }
        if (collision.gameObject.CompareTag("P1Right") && !MrRedMove.FaceLeft)
        {
            MrPinkMove.WalkLeft = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MrRedMove.FaceLeft && collision.gameObject.CompareTag("P1Right"))
        {
            MrPinkMove.WalkRight = true;
        }
        if (collision.gameObject.CompareTag("P1Right"))
        {
            MrPinkMove.WalkLeft = true;
        }
    }
}
