using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private EnemyBehaviour enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
