using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    private Animator anim;
    private EnemyBehaviour enemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<EnemyBehaviour>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        anim.SetTrigger("isDead");
        enemy.moveSpeed = 0f;
        Destroy(gameObject, 2f);
    }
}
