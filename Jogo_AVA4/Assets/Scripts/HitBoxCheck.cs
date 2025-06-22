using UnityEngine;

public class HitBoxCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            // Check if the player is invulnerable
            if (!playerHealth.isInvulnerable)
            {
                // If not invulnerable, apply damage
                playerHealth.TakeDamage(1); // Assuming 1 damage for hitbox check
            }
        }
        // You can add more checks for other components if needed
    }
}
