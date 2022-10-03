using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject damageTrigger;
    public float attackDuration = 0.7f;
    public float attackDelay = 0.5f;
    public Animator animator;
    public AudioClip[] sounds;

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
        damageTrigger.SetActive(true);
        
        animator.SetTrigger("Attack");
        
        _audioSource.clip = RandomAttackSound();
        _audioSource.Play();
        
        StartCoroutine(DeactivateDamageTrigger());
    }

    private AudioClip RandomAttackSound() => sounds[Random.Range(0, sounds.Length)];

    private IEnumerator DeactivateDamageTrigger()
    {
        yield return new WaitForSeconds(attackDuration);
        damageTrigger.SetActive(false);
    }
}
