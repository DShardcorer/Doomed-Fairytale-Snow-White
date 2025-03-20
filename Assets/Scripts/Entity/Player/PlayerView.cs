using UnityEngine;

public class PlayerView : EntityView
{
    private Player _player;
    public Player Player => _player;
    public void Initialize(Player controller)
    {
        base.Initialize(controller);
        _player = controller;
    }

}
