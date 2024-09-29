using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; 
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
    }

    public void DealDamage(int damageAmount)
    {
        currentHealth -= damageAmount; 
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
            // Handle player death (e.g., trigger a death animation, restart level, etc.)
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Provide a way to get the current health
    }

    public int GetMaxHealth()
    {
        return maxHealth; // Provide a way to get the maximum health
    }
}
