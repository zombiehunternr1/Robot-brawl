using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>())
        {
            other.GetComponent<PlayerControl>().Defeated();
        }
    }
}
