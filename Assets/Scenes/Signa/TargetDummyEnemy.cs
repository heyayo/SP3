using System.Collections;
using UnityEngine;
using TMPro;

public class TargetDummyEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float moveDistance = 3.0f; // Distance to move left and right

    private bool isPaused = false;
    private Vector3 originalPosition;
    private Rigidbody2D rb;
    public float MaxHealth = 100.0f;
    private float currentHealth = 0.0f; // Initial health value
    public GameObject damageTextPrefab; // Assign the TextMesh prefab in the Inspector

    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original sprite color
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        StartMovement();
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        if (!isPaused)
        {
            Move();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        // Show damage pop-up
        ShowDamagePopUp(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamagePopUp(float damage)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        TMP_Text damageText = damageTextObject.GetComponent<TMP_Text>();

        if (damageText != null)
        {
            damageText.text = damage.ToString(); // Display the damage value as a whole number
            Destroy(damageTextObject, 1.0f); // Destroy after a certain time
        }
    }
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; // Change the sprite color to red
        yield return new WaitForSeconds(0.2f); // Adjust the duration as needed
        spriteRenderer.color = originalColor; // Revert the sprite color
    }

    private void Die()
    {
        // Implement any death behavior here
        Destroy(gameObject);
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2(Mathf.Sin(Time.time * moveSpeed) * moveDistance, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    public void PauseMovement()
    {
        isPaused = true;
        rb.velocity = Vector2.zero;
    }

    public void ResumeMovement()
    {
        isPaused = false;
        StartMovement();
    }

    private void StartMovement()
    {
        rb.velocity = new Vector2(Mathf.Sin(Time.time * moveSpeed) * moveDistance, rb.velocity.y);
    }

    // Existing methods...
}