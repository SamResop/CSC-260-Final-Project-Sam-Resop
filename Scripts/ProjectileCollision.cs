using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileP1"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
