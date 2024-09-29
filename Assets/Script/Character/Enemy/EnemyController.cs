// Scripts/Characters/Enemy/EnemyController.cs
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private int movingDistance = 5;
    
    [Header("Attack Settings")]
    [SerializeField] private int damageAmount = 10;

    [Header("Components")]
    [SerializeField] private Health health; // Assign via Inspector

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;

    private Transform playerTransform;

    private float maxChaseDistance = 4f; 

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x + movingDistance, initialPosition.y, initialPosition.z);
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void Patrol()
    {
        if (movingToTarget)
        {
            MoveTowards(targetPosition);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                movingToTarget = false;
            }
        }
        else
        {
            MoveTowards(initialPosition);
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                movingToTarget = true;
            }
        }
    }

    // Mengejar player
    private void ChasePlayer()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > maxChaseDistance)
            {
                playerTransform = null;
                return;
            }

            MoveTowards(playerTransform.position);
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    private void OnDeath()
    {
        Debug.Log("Enemy has died.");
    }

    void Awake()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (health != null)
        {
            health.OnDeath += OnDeath;
        }
    }
}
