using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Détection du sol")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    [Header("Tir")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0.2f;
//fin test

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Récupération des inputs WASD
        moveInput = Input.GetAxisRaw("Horizontal"); // A/D par défaut dans Unity

        // Vérifie si le joueur touche le sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saut avec W ou Espace
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Orientation du personnage
        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();
// debut test

        // Tir
        HandleShootInput();

        UpdateAnimations();
    }

    void HandleShootInput()
    {
        bool shootLeft = Input.GetKey(KeyCode.J);
        bool shootUp = Input.GetKey(KeyCode.I);
        bool shootDown = Input.GetKey(KeyCode.K) && !isGrounded; // seulement en l'air
        bool shootRight = Input.GetKey(KeyCode.L);

        bool wantsToShoot = shootRight || shootUp || shootDown || shootLeft;

        if (wantsToShoot && Time.time >= nextFireTime)
        {
            Shoot(shootRight, shootUp, shootDown, shootLeft);
            nextFireTime = Time.time + fireRate;
        }
    }

    Vector2 GetShootDirection(bool right, bool up, bool down, bool left)
    {
        float x = 0f;
        if (right) x += 1f;
        if (left) x -= 1f;

        float y = 0f;
        if (up) y += 1f;
        if (down) y -= 1f;

        if (x == 0f && y == 0f)
        {
            // Aucune direction pressée : tire selon l'orientation du perso
            return facingRight ? Vector2.right : Vector2.left;
        }

        return new Vector2(x, y).normalized;
    }

    void Shoot(bool right, bool up, bool down, bool left)
    {
        Vector2 dir = GetShootDirection(right, up, down, left);
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().SetDirection(dir);

        int aimVertical = 0;
        if (down) aimVertical = -1;
        else if (up) aimVertical = 1;

        animator.SetInteger("AimVertical", aimVertical);
        animator.SetTrigger("Shoot");
    }
// fin du test
    void FixedUpdate()
    {
        // Déplacement horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.Rotate(0f, 180f, 0f);
    }

    void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}