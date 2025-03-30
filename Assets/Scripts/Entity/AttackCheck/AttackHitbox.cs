using Unity.VisualScripting;
using UnityEngine;

public class AttackHitbox : MonoBehaviour, ILifecycle<Entity>
{
    private Entity _parent;
    private EntityProperties _properties;

    [Header("Attack Settings")]
    public AttackType attackType = AttackType.OverlapCircle;
    public Transform attackPoint;

    public float attackRadius = 1.5f;
    public Vector2 attackBoxSize = new Vector2(2f, 1f);
    public Vector2 attackCapsuleSize = new Vector2(2f, 1f);
    public float attackRange = 2f;

    private Vector3 _originalLocalPosition;
    private LayerMask entityLayer;

    [Header("Gizmo Settings")]
    public bool showGizmos = true;

    public void Initialize(Entity parent)
    {
        _parent = parent;
        _properties = _parent.Properties;
        _originalLocalPosition = attackPoint.localPosition;
        entityLayer = HelperLayer.EntityLayerMask;
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

    // === Overloaded PerformAttack Methods === //

    public void PerformAttack(float damage = 20f)
    {
        PerformAttack(attackType, damage);
    }

    public void PerformAttack(AttackType type, float damage)
    {
        switch (type)
        {
            case AttackType.OverlapCircle:
                PerformCircleAttack(damage, attackRadius);
                break;

            case AttackType.OverlapBox:
                PerformBoxAttack(damage, attackBoxSize);
                break;

            case AttackType.OverlapCapsule:
                PerformCapsuleAttack(damage, attackCapsuleSize);
                break;

            case AttackType.Raycast:
                PerformRaycastAttack(damage, attackRange);
                break;
        }
    }

    public void PerformCircleAttack(float damage, float radius)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, radius, entityLayer);
        ProcessHitEntities(hitEntities, damage);
    }

    public void PerformBoxAttack(float damage, Vector2 size)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEntities = Physics2D.OverlapBoxAll(attackPoint.position, size, 0f, entityLayer);
        ProcessHitEntities(hitEntities, damage);
    }

    public void PerformCapsuleAttack(float damage, Vector2 size)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEntities = Physics2D.OverlapCapsuleAll(attackPoint.position, size, CapsuleDirection2D.Horizontal, 0f, entityLayer);
        ProcessHitEntities(hitEntities, damage);
    }

    public void PerformRaycastAttack(float damage, float range)
    {
        if (attackPoint == null) return;

        RaycastHit2D hitRay = Physics2D.Raycast(attackPoint.position, -attackPoint.up, range, entityLayer);
        if (hitRay.collider != null)
        {
            Debug.Log("Hit: " + hitRay.collider.name);
            ApplyDamage(hitRay.collider, damage);
        }
        Debug.DrawRay(attackPoint.position, -attackPoint.up * range, Color.red, 1f);
    }

    // === Helper Methods === //

    private void ProcessHitEntities(Collider2D[] hitEntities, float damage)
    {
        if (hitEntities == null || hitEntities.Length == 0) return;

        foreach (Collider2D entity in hitEntities)
        {
            ApplyDamage(entity, damage);
        }
    }

    private void ApplyDamage(Collider2D entity, float damage)
    {
        if (entity.TryGetComponent<EntityView>(out EntityView e))
        {
            e.Controller.TakeDamage(damage);
            e.Controller.Properties.lastAttacker = _parent;
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
                Gizmos.DrawWireCube(attackPoint.position, attackCapsuleSize);
                break;

            case AttackType.Raycast:
                Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.right * attackRange);
                break;
        }
    }
}
