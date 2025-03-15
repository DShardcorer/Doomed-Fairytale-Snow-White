using UnityEngine;

public class NPCMovingProperties
{
    private float moveSpeed = 2.0f;
    public float MoveSpeed => moveSpeed;

    private float movingTime = 2.0f;
    public float MovingTime => movingTime;

    public Vector2 lastMovementVector;


    public NPCMovingProperties(float moveSpeed = 2.0f, float movingTime = 2.0f)
    {
        this.moveSpeed = moveSpeed;
        this.movingTime = movingTime;
    }
}
