public class PlayerMovementProperties
{
    public float Speed { get; private set; } = 5f;

    public PlayerMovementProperties(float speed = 5f)
    {
        Speed = speed;
    }
}
