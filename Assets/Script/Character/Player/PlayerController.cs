using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;

    private float horizontalAxis;
    private Vector2 direction;

    private bool isGrounded = true;

    void Start()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (health != null)
        {
            // Subscribe to death event if needed
            health.OnDeath += OnDeath;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAnimation();
        HandleAttack();
        HandleFacing();
    }


    private void HandleMovement()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        direction = new Vector2(horizontalAxis, 0);
        transform.Translate(direction * Time.deltaTime * movementSpeed);
    }

    private void HandleAnimation()
    {
        if (horizontalAxis != 0)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }

        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
        }
    }

    private void HandleFacing()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnDeath()
    {
        Debug.Log("Player has died.");
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    int damageAmount = 10;
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log("Dealt damage to enemy!");
                }
            }
        }
    }
}
