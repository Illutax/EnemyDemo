using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 1;
    public int health = 20;
    public float despawnDelay = 2;
    
    // private references
    private Player player;
    private Rigidbody rb;
    private ParticleSystem takingDamageParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>(); // there is only one
        takingDamageParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (IsAlive())
        {
            WalkToPlayer();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null) // did the enemy touch a player?
        {
            player.TakeDamage(1);
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void WalkToPlayer()
    {
        Vector3 myPosition = transform.position;
        Vector3 direction = player.transform.position - myPosition;

        Vector3 movingDirection = direction.normalized * walkSpeed;
        rb.velocity = movingDirection;
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return; // Don't take damage when dead already
        
        takingDamageParticles.Play();
        health = health - amount;

        if (!IsAlive())
        {
            StartCoroutine(Die()); // start dying
        }
    }

    private IEnumerator Die()
    {
        player.GainXP(100); // talk to player that he killed me
        yield return new WaitForSeconds(despawnDelay); // wait
        Despawn();
    }

    private void Despawn()
    {
        Destroy(gameObject); // also destorys everything what is attached to the enemy
    }
}
