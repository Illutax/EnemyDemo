using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 1;
    public int health = 20;
    public float despawnDelay = 2;
    
    // private references
    private Player _player;
    private Rigidbody _rb;
    private ParticleSystem _takingDamageParticles;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>(); // there is only one
        _takingDamageParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!IsAlive()) return;
        
        WalkToPlayer();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsAlive()) return;
        
        Player possiblePlayer = other.gameObject.GetComponent<Player>();

        if (possiblePlayer != null) // did the enemy touch a player?
        {
            this._player.TakeDamage(1);
        }
    }

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

        if (!IsAlive())
        {
            StartCoroutine(Die()); // start dying
        }
    }

    private IEnumerator Die()
    {
        _player.GainXP(100); // talk to player that he killed me
        yield return new WaitForSeconds(despawnDelay);
        Despawn();
    }

    private void Despawn()
    {
        Destroy(gameObject); // also destorys everything what is attached to the enemy
    }
}
