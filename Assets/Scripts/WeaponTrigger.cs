using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null) // did we hit an enemy?
        {
            enemy.TakeDamage(5);
        }
    }
}
