using UnityEngine;

public class DestroyFallingObjects : MonoBehaviour
{
    public LayerMask objectsToDestoryLayerMask;
    public Vector2 size;

    [Header("Gizmo parameters")]
    public Color gizmoColor = Color.red;
    public bool showGizmo = true;

    private void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, size, 0, objectsToDestoryLayerMask);
        if (collider != null)
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Destroy(collider.gameObject);
                return;
            }

            playerHealth.HandleFallDeath();
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position, size);
        }
    }
}
