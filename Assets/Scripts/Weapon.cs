using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The GameObject which is used for damage dealing detection")]
    public GameObject damageTrigger;
    public Animator modelAnimator;
    
    [Header("Attack Choreography")]
    public float attackDuration = 0.7f;
    [Min(1)]
    public int attacksPerSecond = 2;
    public float applyDamageDelay = 0.2f;
    
    [Header("SFX")]
    public AudioClip[] attackSounds;

    // internal references
    private AudioSource _audioSource;
    
    private float _attackDelay;
    private double _nextAttackTime;

    void Start()
    {
        _attackDelay = 1f / attacksPerSecond;
        _audioSource = GetComponent<AudioSource>();
        damageTrigger.SetActive(false);
    }

    public void Attack()
    {
        if (!CanAttack()) return;
        _nextAttackTime = Time.time + _attackDelay;

        StartCoroutine(AttackChoreography());
    }

    private bool CanAttack()
    {
        return Time.time > _nextAttackTime;
    }

    private IEnumerator AttackChoreography()
    {
        modelAnimator.SetTrigger("Attack");

        _audioSource.clip = RandomAttackSound();
        _audioSource.Play();

        yield return new WaitForSeconds(applyDamageDelay);
        damageTrigger.SetActive(true);
        yield return new WaitForSeconds(attackDuration - applyDamageDelay);
        damageTrigger.SetActive(false);
    }

    private AudioClip RandomAttackSound() => attackSounds[Random.Range(0, attackSounds.Length)];

}
