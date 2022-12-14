using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupArea : MonoBehaviour
{
    public Weapon thisWeapon;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null) // did an player tried to pick it up?
        {
            player.weapon = thisWeapon;
        }
    }
}
