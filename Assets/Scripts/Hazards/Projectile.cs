using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    private Action<Projectile> _releaseAction;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>())
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            player.StartDizzynessEffect();
        }
        collision.gameObject.GetComponent<Tile>().isTargetable = true;
        _releaseAction(this);
    }

    public void setReleaseAction(Action<Projectile> releaseAction)
    {
        _releaseAction = releaseAction;
    } 
}
