using UnityEngine;

public class HealthSystemView : MonoBehaviour, ILifecycle<HealthSystem>
{
    private HealthSystem _healthSystem;

    public void Initialize(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _healthSystem.LoseHealth(10);
            Debug.Log("Current Health: " + _healthSystem.GetCurrentHealth());
        }
    }

    public void Dispose()
    {
        _healthSystem = null;
    }
}

