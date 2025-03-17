using UnityEngine;

public class AttackHitbox : MonoBehaviour, ILifecycle<Entity>
{
    private Entity _parent;
    private EntityProperties _properties;
    [Header("Attack Settings")]
    public AttackType attackType; // Select attack type in Inspector

    public Transform attackPoint; // Set attack origin in the Inspector

    public float attackRadius = 1.5f; // For OverlapCircle
    public Vector2 attackBoxSize = new Vector2(2f, 1f); // For OverlapBox
    public Vector2 attackCapsuleSize = new Vector2(2f, 1f); // For OverlapCapsule
    public float attackRange = 2f; // For Raycast (spears, ranged attacks)
    private Vector3 _originalLocalPosition;
    private LayerMask entityLayer;

    [Header("Gizmo Settings")]
    public bool showGizmos = true; // Toggle gizmos
    public void Initialize(Entity parent)
    {
        _parent = parent;
        _properties = _parent.Properties;
        attackType = _properties.AttackType;
        attackRadius = _properties.AttackRadius;
        attackBoxSize = _properties.AttackBoxSize;
        attackCapsuleSize = _properties.AttackCapsuleSize;
        attackRange = _properties.AttackRange;
        _originalLocalPosition = attackPoint.localPosition;
        entityLayer = LayerHelper.EntityLayerMask;

    }
    public void Dispose()
    {
        _parent = null;
        _properties = null;
    }
    public void SetAttackHitBoxRotation(Vector2 direction)
    {
        transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, direction));
    }

    public void PerformAttack(AttackType type)
    {
        attackType = type;
        PerformAttack();
    }


    public void PerformAttack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack point is not assigned!");
            return;
        }

        Collider2D[] hitEntities = null;
        RaycastHit2D hitRay = default;

        switch (attackType)
        {
            case AttackType.OverlapCircle:
                hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, entityLayer);
                break;

            case AttackType.OverlapBox:
                hitEntities = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0f, entityLayer);
                break;

            case AttackType.OverlapCapsule:
                hitEntities = Physics2D.OverlapCapsuleAll(attackPoint.position, attackCapsuleSize, CapsuleDirection2D.Horizontal, 0f, entityLayer);
                break;

            case AttackType.Raycast:
                hitRay = Physics2D.Raycast(attackPoint.position, attackPoint.right, attackRange, entityLayer);
                if (hitRay.collider != null)
                {
                    Debug.Log("Hit: " + hitRay.collider.name);
                    // Apply damage or effect
                }
                return; // Skip the loop since it's a single target hit
        }
        if (hitEntities == null)
        {
            return;
        }
        // Process detected entities
        if (hitEntities != null)
        {
            foreach (Collider2D entity in hitEntities)
            {
                Debug.Log("Hit: " + entity.name);
                if (entity.TryGetComponent<EntityView>(out EntityView e))
                {
                    e.Controller.TakeDamage(_parent.Properties.AttackDamage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || attackPoint == null) return;

        Gizmos.color = Color.red;

        switch (attackType)
        {
            case AttackType.OverlapCircle:
                Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
                break;

            case AttackType.OverlapBox:
                Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
                break;

            case AttackType.OverlapCapsule:
                Gizmos.DrawWireCube(attackPoint.position, attackCapsuleSize); // Placeholder for capsule
                break;

            case AttackType.Raycast:
                Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.right * attackRange);
                break;
        }
    }
}
