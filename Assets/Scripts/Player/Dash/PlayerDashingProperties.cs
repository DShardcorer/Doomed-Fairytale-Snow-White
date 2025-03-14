using UnityEngine;

public class PlayerDashingProperties
{
    private float _dashSpeed = 10f;

    public float DashSpeed => _dashSpeed;

    public PlayerDashingProperties(float dashSpeed = 10f)
    {
        _dashSpeed = dashSpeed;
    }

}
