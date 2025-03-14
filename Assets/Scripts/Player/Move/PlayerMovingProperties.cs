using UnityEngine;

public class PlayerMovingProperties
{
    private float _moveSpeed = 5f;
    public float MoveSpeed => _moveSpeed;

    public Vector2 lastMovementVector;

    public PlayerMovingProperties(float moveSpeed = 5f)
    {
        _moveSpeed = moveSpeed;
    }

}
