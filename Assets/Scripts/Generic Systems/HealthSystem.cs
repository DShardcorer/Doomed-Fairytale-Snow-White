
public class HealthSystem : ILifecycle<Player>
{
    private Player _player;
    private HealthSystemProperties _healthSystemProperties;
    private HealthSystemView _healthSystemView;

    public HealthSystemProperties HealthSystemProperties => _healthSystemProperties;
    public HealthSystemView HealthSystemView => _healthSystemView;

    public HealthSystem(HealthSystemProperties healthSystemProperties, HealthSystemView healthSystemView)
    {
        _healthSystemProperties = healthSystemProperties;
        _healthSystemView = healthSystemView;
    }

    public HealthSystem()
    {
    }

    public void Initialize(Player player)
    {
        _player = player;
        _healthSystemView.Initialize(this);
        
    }
        public void Dispose()
    {
        _player = null;
        _healthSystemView.Dispose();
    }


    public void LoseHealth(int health)
    {
        _healthSystemProperties.SetHealth(_healthSystemProperties.CurrentHealth - health);
    }

    public float GetCurrentHealth()
    {
        return _healthSystemProperties.CurrentHealth;
    }
}

