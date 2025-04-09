using System;
using UnityEngine;

[Serializable]
public class NPCMovingProperties: EntityStateProperties
{
    private float moveSpeed = 2.0f;
    public float MoveSpeed => moveSpeed;

    private float movingTime = 2.0f;
    public float MovingTime => movingTime;

    public NPCMovingProperties(float moveSpeed = 2.0f, float movingTime = 2.0f)
    {
        this.moveSpeed = moveSpeed;
        this.movingTime = movingTime;
    }

    protected override void UpdateDerivedProperties(object sender, EventArgs e)
    {
        
    }
}
