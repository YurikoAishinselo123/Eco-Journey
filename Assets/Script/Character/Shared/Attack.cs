using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAmount = 10;

    public void PerformAttack(Health targetHealth)
    {
        if (targetHealth != null)
        {
            Damage damage = new Damage(damageAmount, transform.position, gameObject);
            targetHealth.TakeDamage(damage.DamageAmount);
        }
    }
}
