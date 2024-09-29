using UnityEngine;

public struct Damage
{
    public int DamageAmount { get; }
    public Vector3 HitPoint { get; }
    public GameObject Source { get; }

    public Damage(int damageAmount, Vector3 hitPoint, GameObject source)
    {
        DamageAmount = damageAmount;
        HitPoint = hitPoint;
        Source = source;
    }
}
