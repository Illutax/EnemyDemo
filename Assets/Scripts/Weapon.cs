using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public GameObject damageTrigger;
    public Animator animator;
    [Space]
    
    [Header("Attack Choreography")]
    public float attackDuration = 0.7f;
    public float attackDelay = 0.5f;
    public float applyDamageDelay = 0.2f;
    
    [Header("SFX")]
    public AudioClip[] attackSounds;

    private AudioSource _audioSource;
    private double _lastAttackTime;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        damageTrigger.SetActive(false);
    }

    public void Attack()
    {
        if (Time.time < _lastAttackTime + attackDelay) return;
        _lastAttackTime = Time.time;

        StartCoroutine(AttackChoreography());
    }

    private IEnumerator AttackChoreography()
    {
        animator.SetTrigger("Attack");

        _audioSource.clip = RandomAttackSound();
        _audioSource.Play();

        yield return new WaitForSeconds(applyDamageDelay);
        damageTrigger.SetActive(true);
        yield return new WaitForSeconds(attackDuration - applyDamageDelay);
        damageTrigger.SetActive(false);
    }

    private AudioClip RandomAttackSound() => attackSounds[Random.Range(0, attackSounds.Length)];

}
