using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthBar;
    public GameObject gameOverScreen;

    [Header("Invincibilité après un coup")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead || isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
            healthBar.value = currentHealth;

        Debug.Log("Joueur touché ! Vie restante : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    System.Collections.IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;
        float flashInterval = 0.1f;

        while (elapsed < invincibilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvincible = false;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Le joueur est mort");

        if (playerController != null)
            playerController.enabled = false;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Death");

        StartCoroutine(ShowGameOverAfterDelay(2f));
    }

    System.Collections.IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f; // met le jeu en pause
    }


}