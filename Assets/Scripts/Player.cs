using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public Weapon weapon;
    
    [Header("Movement")]
    public float moveSpeed = 3;
    
    [Header("Stats")]
    public int health = 100;
    
    [Header("SFX")]
    public AudioClip[] hitSounds;
    
    [Space]
    [Header("Only for displaying")]
    public int xp;
    
    // private references
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private ParticleSystem _takingDamageParticles;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _takingDamageParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!IsAlive())
        {
            print("Dead can't move");
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Attack();
        }
        Movement();
    }

    private void Movement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var direction = new Vector3(x, 0, y);
        Walk(direction);
        if (x != 0 || y != 0) // otherwise the player snaps to looking up
        {
            Look(direction);
        }
    }

    private void Walk(Vector3 direction)
    {
        _rb.velocity = direction.normalized * moveSpeed;
    }

    private void Look(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void GainXp(int amount)
    {
        xp = xp + amount;
        print("Current xp " + xp);
    }

    private bool IsAlive()
    {
        return health > 0;
    }
    
    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return; // take no damage when dead already
        
        PlayRandomHitSound();
        _takingDamageParticles.Play();
        health = health - amount;
        
        if (!IsAlive())
        {
            StartCoroutine(GameOver());
        }
    }

    private void PlayRandomHitSound()
    {
        _audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        _audioSource.Play();
    }

    private IEnumerator GameOver()
    {
        print("You died!");
        yield return new WaitForSeconds(5);
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
