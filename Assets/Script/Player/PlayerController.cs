using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Add this for using UI elements like Slider

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumlahLompatan = 2;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider healthSlider; // Reference to the health slider
    [SerializeField] private int attackDamage = 10; // Damage dealt by the player

    private float horizontalAxis;
    private Vector2 direction;
    private HealthManager healthManager; // Reference to the HealthManager

    void Start()
    {
        healthManager = GetComponent<HealthManager>(); // Get the HealthManager component
        if (healthSlider != null)
        {
            healthSlider.maxValue = healthManager.GetMaxHealth(); // Set the slider's max value
            healthSlider.value = healthManager.GetCurrentHealth(); // Initialize the slider value
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement();
        facing();
        Jump();
        Attack(); // Check for attack input
        UpdateHealthSlider(); // Update the slider value every frame
    }

    private void movement()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        direction = new Vector2(horizontalAxis, 0);
        transform.Translate(direction * Time.deltaTime * movementSpeed);

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

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumlahLompatan > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumlahLompatan = jumlahLompatan - 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D Player)
    {
        if(Player.gameObject.CompareTag("Ground"))
        {
            jumlahLompatan = 2;
        }

        else if(Player.gameObject.CompareTag("Enemy"))
        {
            jumlahLompatan = 2;
        }

        else if (Player.gameObject.CompareTag("Obstacle"))
        {
            jumlahLompatan = 2;
        }
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthManager.GetCurrentHealth(); // Update the slider value based on current health
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f); // Check in front of the player
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                HealthManager enemyHealth = hit.collider.GetComponent<HealthManager>();
                if (enemyHealth != null)
                {
                    enemyHealth.DealDamage(attackDamage); // Deal damage to the enemy
                }

                animator.SetTrigger("Punch");
            }
        }
    }

    void facing()
    {
        if(horizontalAxis < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else if(horizontalAxis > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}