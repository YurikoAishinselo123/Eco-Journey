using UnityEngine;
using UnityEngine.UI; // Add this for using UI elements like Slider

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider healthSlider; // Reference to the health slider

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
        Movement();
        Jump();
        UpdateHealthSlider(); // Update the slider value every frame
    }

    void Movement()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        direction = new Vector2(horizontalAxis, 0);

        if (horizontalAxis != 0)
        {
            animator.SetBool("PlayerWalk", true);
            animator.SetBool("PlayerIdle", false);
            transform.Translate(direction * Time.deltaTime * movementSpeed);
        }

        else
        {
            animator.SetBool("PlayerIdle", true);
            animator.SetBool("PlayerWalk", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply a force for jumping
        }
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthManager.GetCurrentHealth(); // Update the slider value based on current health
        }
    }
}
