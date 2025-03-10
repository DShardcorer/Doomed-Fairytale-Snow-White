using UnityEngine;

public class PlayerMovementView : MonoBehaviour, ILifecycle<PlayerMovement>
{
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;
    public void Initialize(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 movementVector)
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + movementVector);
    }

    public void Dispose()
    {
        _playerMovement = null;
    }


}
