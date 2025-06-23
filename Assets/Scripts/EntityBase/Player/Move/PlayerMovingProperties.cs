using System;

namespace EntityBase.Player.Move
{
    public class PlayerMovingProperties: EntityStateProperties
    {
        private float _moveSpeed = 5f;
        public float MoveSpeed => _moveSpeed;



        public PlayerMovingProperties(float moveSpeed = 5f)
        {
            _moveSpeed = moveSpeed;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}
