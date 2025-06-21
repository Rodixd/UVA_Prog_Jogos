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

    [field: SerializeField]
    private UnityEvent onRespawnRequired { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        respawnHelper = GetComponent<RespawnHelper>();
        originalScale = transform.localScale;

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
        RespawnPoint firstSpawn = manager.transform.GetChild(0).GetComponent<RespawnPoint>();
        manager.ForceRespawnAt(firstSpawn, gameObject);

        state = State.idle;
        anim.Play("Player_Idle");
        anim.SetInteger("State", (int)state);
    }

    void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (hDirection < 0)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            state = State.jump;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.attack;
            anim.SetTrigger("Attack");
        }

        VelocityState();
        anim.SetInteger("State", (int)state);
    }

    public void PlayerDied()
    {
        onRespawnRequired?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            key += 1;
        }
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
}
