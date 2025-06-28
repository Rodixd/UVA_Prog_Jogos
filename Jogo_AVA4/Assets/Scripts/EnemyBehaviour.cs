using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject attackHitbox;
    public int damage = 1;

    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;

    private Transform visualChild;
    private Vector3 originalScale;
    private Vector3 originalTriggerLocalPos;
   
    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            visualChild = sr.transform;
            originalScale = visualChild.localScale;
        }
        else
        {
            Debug.LogWarning("EnemyBehaviour: No SpriteRenderer found in children!");
            visualChild = transform;  // fallback
            originalScale = visualChild.localScale;
        }
        if(triggerArea != null)
        {
            originalTriggerLocalPos = triggerArea.transform.localPosition;
        }
        else
        {
            Debug.LogWarning("EnemyBehaviour: No triggerArea assigned!");
        }
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }
    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }


    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            Flip();

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;
        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft < distanceToRight)
        {
            target = rightLimit;
        }
        else
        {
            target = leftLimit;
        }

        Flip();
    }

    public void Flip()
    {
        if (visualChild == null) return;

        Vector3 scale = originalScale;

        if (inRange && target != null)
        {
            if (transform.position.x < target.position.x)
                scale.x = -Mathf.Abs(originalScale.x);  // face left if target is right
            else
                scale.x = Mathf.Abs(originalScale.x);   // face right if target is left
        }
        else if (target != null)
        {
            if (transform.position.x < target.position.x)
                scale.x = -Mathf.Abs(originalScale.x);
            else
                scale.x = Mathf.Abs(originalScale.x);
        }

        visualChild.localScale = scale;

        if(triggerArea != null)
        {
            Vector3 triggerPos = triggerArea.transform.localPosition;

            if(scale.x > 0)
            {
                triggerPos.x = Mathf.Abs(originalTriggerLocalPos.x);
            }
            else
            {
                triggerPos.x = -Mathf.Abs(originalTriggerLocalPos.x);
            }
            triggerArea.transform.localPosition = triggerPos;
        }
    }
}
