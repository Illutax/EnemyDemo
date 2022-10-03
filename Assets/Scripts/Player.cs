using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Weapon weapon;
    public float moveSpeed = 3;
    public int health = 100;
    
    [Space]
    [Header("Only for displaying")]
    public int xp;
    
    // private references
    private Rigidbody rb;
    private ParticleSystem takingDamageParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // for movement
        takingDamageParticles = GetComponent<ParticleSystem>();
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
            Attack();
        }
        Movement();
    }

    private void Attack()
    {
        weapon.Attack();
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
        rb.velocity = direction.normalized * moveSpeed;
    }

    private void Look(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void GainXP(int amount)
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
        
        takingDamageParticles.Play();
        health = health - amount;
        print("Ouch! Current HP = " + health);
        
        if (!IsAlive())
        {
            StartCoroutine(GameOver());
        }
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
