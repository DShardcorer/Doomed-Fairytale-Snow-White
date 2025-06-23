using EntitySystems.WeaponSystem;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    public class WeaponView: MonoBehaviour, ILifecycle<Weapon>
    {
        private Weapon _parent;
        private const string IS_ATTACKING = "isAttacking";
        
        [SerializeField] private Animator animator;
        public Animator Animator => animator;


        public void Initialize(Weapon parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent = null;
            animator = null;
        }
        public void SetIsAttacking(bool isAttacking)
        {
            if (animator != null)
            {
                animator.SetBool(IS_ATTACKING, isAttacking);
            }
            else
            {
                Debug.LogError("Animator is not assigned in WeaponView.");
            }
        }
        
    }
}