using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public int damageAmount = 5;

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null) // did we hit an enemy?
        {
            enemy.TakeDamage(damageAmount);
        }
    }
}
