using UnityEngine;
using UnityEngine.UI; // Add this for using UI elements like Slider

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Slider healthSlider; // Reference to the health slider

    private HealthManager healthManager; // Reference to the HealthManager

    void Start()
    {
        healthManager = GetComponent<HealthManager>(); // Get the HealthManager component
        if (healthSlider != null)
        {
            healthSlider.maxValue = healthManager.GetMaxHealth(); // Set the slider's max value
            healthSlider.value = healthManager.GetCurrentHealth(); // Initialize the slider value
        }
    }

    void Update()
    {
        Movement();
        Jump();
        UpdateHealthSlider(); // Update the slider value every frame
    }

    void Movement()
    {
        // Vector2 for horizontal movement
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);
        rb.velocity = movement; // Update the Rigidbody's velocity
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
