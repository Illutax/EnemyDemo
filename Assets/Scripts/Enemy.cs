using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed = 1;
    [Min(1)]
    public int attacksPerSecond = 2;
    public int health = 20;
    
    [Header("Misc")]
    public float despawnDelay = 2;
    
    [Header("SFX")]
    public AudioClip[] hitSounds;

    // private references
    private Player _player;
    private Rigidbody _rb;
    private ParticleSystem _takingDamageParticles;
    private Collider _collider;
    private AudioSource _audioSource;
    
    private const int EnemyLayer = 6;
    private float _attackDelay;
    private float _nextAttackTime;

    void Start()
    {
        _attackDelay = 1f / attacksPerSecond;
        _player = FindObjectOfType<Player>();
        
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _takingDamageParticles = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsAlive()) return;

        WalkToPlayer();
    }

    private void OnCollisionStay(Collision other)
    {
        if (!IsAlive()) return;
        if (!CanAttack()) return;
        if (other.gameObject.layer == EnemyLayer) Physics.IgnoreCollision(_collider, other.collider);

        Player possiblePlayer = other.gameObject.GetComponent<Player>();

        if (possiblePlayer != null) // did the enemy touch a player?
        {
            _nextAttackTime = Time.time + _attackDelay;
            _player.TakeDamage(1);
        }
    }

    private bool CanAttack() => Time.time > _nextAttackTime;

    public bool IsAlive()
    {
        return health > 0;
    }

    private void WalkToPlayer()
    {
        Vector3 myPosition = transform.position;
        Vector3 direction = _player.transform.position - myPosition;

        Vector3 movingDirection = direction.normalized * walkSpeed;
        _rb.velocity = movingDirection;
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return; // Don't take damage when dead already

        _takingDamageParticles.Play();
        health = health - amount;
        PlayRandomHitSound();
        
        if (!IsAlive())
        {
            StartCoroutine(Die()); // start dying
        }
    }

    private void PlayRandomHitSound()
    {
        _audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        _audioSource.Play();
    }

    private IEnumerator Die()
    {
        _player.GainXp(100); // talk to player that he killed me
        yield return new WaitForSeconds(despawnDelay);
        Despawn();
    }

    private void Despawn()
    {
        Destroy(gameObject); // also destorys everything what is attached to the enemy
    }
}