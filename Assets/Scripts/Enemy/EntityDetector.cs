using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _collider;

    public CapsuleCollider2D Collider2D => _collider;

    public void OnTiggerEnter2D(Collider2D collision)
    {

    }

    public void SetColliderRotation(float angle)
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
private void OnDrawGizmosSelected()
    {
        if (_collider == null)
            return;

        // Save the current Gizmos matrix
        Matrix4x4 oldMatrix = Gizmos.matrix;
        
        // Use the collider's transform matrix so the gizmo is positioned correctly.
        Gizmos.matrix = _collider.transform.localToWorldMatrix;
        
        // Get collider parameters.
        Vector2 size = _collider.size;
        Vector2 offset = _collider.offset;
        float direction = _collider.direction == CapsuleDirection2D.Horizontal ? 0f : 90f;
        
        // Set Gizmos color.
        Gizmos.color = Color.green;
        
        // Draw a wire capsule. Since Unity doesn't have a built-in method for drawing a capsule, 
        // we can approximate it with a combination of wire spheres and a rectangle.
        // For simplicity, we'll draw a wire sphere at the collider's offset.
        Gizmos.DrawWireSphere(offset, size.x * 0.5f);
        
        // Reset Gizmos matrix.
        Gizmos.matrix = oldMatrix;
    }

}
