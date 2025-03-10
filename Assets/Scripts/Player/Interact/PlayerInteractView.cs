using UnityEngine;

public class PlayerInteractView: MonoBehaviour, ILifecycle<PlayerInteract>
{
    private PlayerInteract _playerInteract;

    public void Dispose()
    {
        _playerInteract = null;
    }

    public void Initialize(PlayerInteract parent)
    {
        _playerInteract = parent;
    }

}
