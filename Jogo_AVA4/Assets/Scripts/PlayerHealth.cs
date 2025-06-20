using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;

    public HealthUI healthUI;
    private Animator anim;

    public UnityEvent onPlayerDeath;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (onPlayerDeath == null)
            onPlayerDeath = new UnityEvent();
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

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            healthUI.SetHealth(currentHealth);
            anim.SetBool("isHit", true);

            if (currentHealth <= 0)
            {
                StartCoroutine(DieAndRespawnRoutine(true));  // Full level reset
            }
        }
    }

    public void HandleFallDeath()
    {
        currentHealth -= 1;
        healthUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndRespawnRoutine(true));  // Full reset
        }
        else
        {
            GetComponent<PlayerController>().RespawnAfterDeath();
        }
    }

    private IEnumerator DieAndRespawnRoutine(bool resetToFirstSpawn)
    {
        anim.SetTrigger("Die");

        onPlayerDeath?.Invoke();

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        currentHealth = maxHealth;
        healthUI.SetHealth(currentHealth);

        if (resetToFirstSpawn)
        {
            GetComponent<PlayerController>().RespawnAtFirstSpawn();
        }
        else
        {
            GetComponent<PlayerController>().RespawnAfterDeath();
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
