using UnityEngine;
using UnityEngine.Events;
using RespawnSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerHealth playerHealth;
    private RespawnHelper respawnHelper;

    private enum State { idle, run, jump, falling, attack };
    private State state = State.idle;
    private Collider2D coll;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public int key = 0;
    private Vector3 originalScale;
    public int damage = 1;

    [field: SerializeField]
    private UnityEvent onRespawnRequired { get; set; }

    [Header("UI")]
    public GameObject keyIconUI;

    [Header("Attack Hitbox")]
    public GameObject attackHitboxObject;  // Assign your AttackHitbox GameObject here in inspector
    private Vector3 attackHitboxOriginalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        respawnHelper = GetComponent<RespawnHelper>();
        originalScale = transform.localScale;

        if (attackHitboxObject != null)
        {
            attackHitboxOriginalPos = attackHitboxObject.transform.localPosition;
            attackHitboxObject.SetActive(false);
        }

        onRespawnRequired.AddListener(() =>
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            if (respawnHelper != null)
            {
                respawnHelper.RespawnPlayer();
            }
        });

        if (playerHealth != null)
        {
            playerHealth.onPlayerDeath.AddListener(() =>
            {
                RespawnAfterDeath();
            });
        }

        if (keyIconUI != null)
        {
            keyIconUI.SetActive(false);
        }
    }

    void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

        // Horizontal Movement and Flipping
        if (hDirection > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            FlipAttackHitbox(true);
        }
        else if (hDirection < 0)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            FlipAttackHitbox(false);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            state = State.jump;
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.attack;
            anim.SetTrigger("Attack");
        }

        VelocityState();
        anim.SetInteger("State", (int)state);
    }

    private void FlipAttackHitbox(bool facingRight)
    {
        if (attackHitboxObject == null) return;

        Vector3 pos = attackHitboxOriginalPos;
        pos.x = facingRight ? Mathf.Abs(attackHitboxOriginalPos.x) : -Mathf.Abs(attackHitboxOriginalPos.x);
        attackHitboxObject.transform.localPosition = pos;
    }

    private void VelocityState()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return;
        }

        if (state == State.jump)
        {
            if (rb.linearVelocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(Ground))
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.linearVelocity.x) > 2f)
        {
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }

    public void PlayerDied()
    {
        onRespawnRequired?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            key += 1;

            if (keyIconUI != null)
            {
                keyIconUI.SetActive(true);
            }
        }
    }

    public void RespawnAfterDeath()
    {
        if (respawnHelper != null)
        {
            respawnHelper.RespawnPlayer();
        }

        state = State.idle;
        anim.Play("Player_Idle");
        anim.SetInteger("State", (int)state);
    }

    public void RespawnAtFirstSpawn()
    {
        RespawnPointManager manager = FindFirstObjectByType<RespawnPointManager>();

        manager.ResetToFirstRespawnPoint();

        RespawnPoint firstSpawn = manager.transform.GetChild(0).GetComponent<RespawnPoint>();
        manager.ForceRespawnAt(firstSpawn, gameObject);

        state = State.idle;
        anim.Play("Player_Idle");
        anim.SetInteger("State", (int)state);
    }

    public void FallDeath()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }

    // Call from Animation Event
    public void EnableAttackHitbox()
    {
        if (attackHitboxObject != null)
        {
            attackHitboxObject.SetActive(true);
        }
    }

    // Call from Animation Event
    public void DisableAttackHitbox()
    {
        if (attackHitboxObject != null)
        {
            attackHitboxObject.SetActive(false);
        }
    }
}
