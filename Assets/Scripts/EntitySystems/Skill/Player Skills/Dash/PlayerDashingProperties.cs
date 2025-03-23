using System;
using UnityEngine;

public class PlayerDashingProperties: EntityStateProperties
{
    private float _dashSpeed = 10f;

    public float DashSpeed => _dashSpeed;

    public PlayerDashingProperties(float dashSpeed = 10f)
    {
        _dashSpeed = dashSpeed;
    }

    protected override void UpdateDerivedProperties(object sender, EventArgs e)
    {
    }
}
