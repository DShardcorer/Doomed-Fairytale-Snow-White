using System;
using UnityEngine;

public class PlayerShootingState : PlayerState
{
    private PlayerShootingProperties _playerShootingProperties;
    public PlayerShootingState(PlayerShootingProperties playerShootingProperties, string animationBoolName) : base(animationBoolName)
    {
        _playerShootingProperties = playerShootingProperties;
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.IsBusy = true;

    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if (!_isAnimationEnded)
        {

        }
        else
        {
            _stateMachine.ChangeState(_player.PlayerIdlingState);
        }
    }
    protected override void OnTakingEffect(object sender, EventArgs e)
    {
        _entity.AttackHitbox.PerformRaycastAttack(_playerShootingProperties.ShootDamage, _playerShootingProperties.ShootRange);
    }
    public override void ExitState()
    {
        _player.IsBusy = false;
        base.ExitState();
    }



}
