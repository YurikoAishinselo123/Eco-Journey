using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private int movingPosition;
    [SerializeField] private int attackDamage = 10; // Damage dealt by the enemy
    [SerializeField] private Slider healthSlider; // Reference to the enemy health slider
    [SerializeField] private float cooldownTime = 2f; // Time between attacks

    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool movingToTarget = true;
    private bool attackPlayer = false;

    private Transform playerTransform; // Reference to the player's transform
    private float maxChaseDistance = 4f; // Maximum distance to chase the player
    private float distanceMoved = 0f;    // Track how far the enemy has moved
    private float nextAttackTime = 0f;   // Time when the enemy can attack again

    private HealthManager healthManager; // Reference to the enemy's health manager

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x + movingPosition, initialPosition.y, 0);
        healthManager = GetComponent<HealthManager>(); // Get the enemy's HealthManager

        if (healthSlider != null)
        {
            healthSlider.maxValue = healthManager.GetMaxHealth(); // Initialize the slider
            healthSlider.value = healthManager.GetCurrentHealth();
        }
    }

    void Update()
    {
        if (!attackPlayer)
        {
            BackandForth();
        }
        else
        {
            ChasePlayer();
        }

        UpdateHealthSlider();
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthManager.GetCurrentHealth();
        }
    }

    void BackandForth()
    {
        if (movingToTarget)
        {
            MoveTowards(targetPosition);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingToTarget = false;
            }
        }
        else
        {
            MoveTowards(initialPosition);
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                movingToTarget = true;
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        if (playerTransform != null)
        {
            float movementThisFrame = Mathf.Abs(transform.position.x - initialPosition.x);
            distanceMoved = movementThisFrame;

            // Stop chasing the player if the enemy has moved 4 units along the x-axis
            if (distanceMoved < maxChaseDistance)
            {
                MoveTowards(playerTransform.position); // Continue chasing player
            }
            else
            {
                attackPlayer = false; // Stop chasing and return to back-and-forth movement
                playerTransform = null; // Clear the player reference when stopping
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attackPlayer = true; // Indicate that the enemy should attack
            playerTransform = other.transform; // Set player transform when player is detected
            distanceMoved = 0f; // Reset distance moved when starting chase
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();
            if (healthManager != null && Time.time >= nextAttackTime) // Check if the cooldown is over
            {
                healthManager.DealDamage(attackDamage); // Call the method to deal damage
                nextAttackTime = Time.time + cooldownTime; // Set the next available attack time
            }
        }
    }
}
