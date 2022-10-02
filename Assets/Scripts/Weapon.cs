using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject damageTrigger;
    public float attackDuration = 0.7f;

    void Start()
    {
        damageTrigger.SetActive(false);
    }

    public void Attack()
    {
        damageTrigger.SetActive(true);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(attackDuration);
        damageTrigger.SetActive(false);
    }
}
