using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Comportement")]
    public float moveSpeed = 2f;
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("Zones (Trigger)")]
    public Collider2D detectionCollider;
    public Collider2D attackCollider;

    private Rigidbody2D rb;
    private Transform player;
    private Collider2D playerCollider;
    private float nextAttackTime = 0f;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();

        bool playerInAttackRange = playerCollider != null && playerCollider.IsTouching(attackCollider);

        if (playerInAttackRange)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            float direction = player.position.x > transform.position.x ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.isTrigger) return;
        player = other.transform;
        playerCollider = other;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.isTrigger) return;
        if (!other.IsTouching(detectionCollider))
        {
            player = null;
            playerCollider = null;
        }
    }

    void Attack()
    {
        if (player == null) return;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}