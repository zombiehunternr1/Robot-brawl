using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>())
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            player.StartDizzynessEffect();
        }
        Destroy(gameObject);
    }
}
