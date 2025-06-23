using UnityEngine;

public class HitBoxCheck : MonoBehaviour
{
    public int damage = 1;
    private bool hasDealtDamage = false;

    private void OnEnable()
    {
        // Reset at the start of each attack
        hasDealtDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsInvulnerable())
        {
            playerHealth.TakeDamage(damage);
            hasDealtDamage = true;
        }
    }
}
