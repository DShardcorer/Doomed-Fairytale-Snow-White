
public class EnemyProperties
{
    private float maxHealth;
    private float currentHealth;
    private float moveSpeed;
    private float attackRange;
    private float attackDamage;
    private float attackCooldown;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    public EnemyProperties(EnemyPropertiesSO enemyStatsSO){
        maxHealth = enemyStatsSO.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = enemyStatsSO.moveSpeed;
        attackRange = enemyStatsSO.attackRange;
        attackDamage = enemyStatsSO.attackDamage;
        attackCooldown = enemyStatsSO.attackCooldown;
    }
}
