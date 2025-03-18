using UnityEngine;

public class PlayerShootingProperties
{
    private float _shootDamage = 20;
    private float _shootRange = 100;
    private float _shootKnockbackForce = 10;

    public float ShootDamage => _shootDamage;
    public float ShootRange => _shootRange;
    public float ShootKnockbackForce => _shootKnockbackForce;

    public PlayerShootingProperties(float shootDamage, float shootRange, float shootKnockbackForce)
    {
        _shootDamage = shootDamage;
        _shootRange = shootRange;
        _shootKnockbackForce = shootKnockbackForce;
    }
}
