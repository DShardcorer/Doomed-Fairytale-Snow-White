
public class HealthSystemProperties
{

    private float _maxHealth;
    private float _currentHealth;

    public HealthSystemProperties(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public float MaxHealth => _maxHealth;

    public float CurrentHealth => _currentHealth;

    public void SetHealth(float health)
    {
        _currentHealth = health;
    }

}
