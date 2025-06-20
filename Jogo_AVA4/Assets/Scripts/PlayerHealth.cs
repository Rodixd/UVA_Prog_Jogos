using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;

    public HealthUI healthUI;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.Initialize(maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
        Trap trap = collision.GetComponent<Trap>();
        if (trap && trap.damage > 0 && !isInvulnerable)
        {
            TakeDamage(trap.damage);
            StartCoroutine(BecomeInvulnerable());
        }
    }

    private void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            healthUI.SetHealth(currentHealth);
        }
        
        // Hit animation
        anim.SetBool("isHit", true);


        if (currentHealth <= 0)
        {
            //Player morreu, game over
        }
    }

    public void HitTrigger()
    {
        anim.SetBool("isHit", false);
    }

    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;

    }
}
