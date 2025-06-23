
using EntityBase;
using GeneralManagers;

namespace EntitySystems.WeaponSystem
{
    public class WeaponSystem: ILifecycle<Entity>
    {
        private Entity _parent;
        public Entity Parent => _parent;
        private Weapon _primaryWeapon;
        public Weapon PrimaryWeapon => _primaryWeapon;
        private Weapon _secondaryWeapon;
        public Weapon SecondaryWeapon => _secondaryWeapon;
        
        public void Initialize(Entity parent)
        {
            _parent = parent;
            WeaponView primaryWeaponView = _parent.View.PrimaryWeaponView;
            if (primaryWeaponView != null)
            {
                _primaryWeapon = new Weapon();
                _primaryWeapon.Initialize(this, primaryWeaponView);
            }
            else
            {
                _primaryWeapon = null;
            }
            WeaponView secondaryWeaponView = _parent.View.SecondaryWeaponView;
            if (secondaryWeaponView != null)
            {
                _secondaryWeapon = new Weapon();
                _secondaryWeapon.Initialize(this, secondaryWeaponView);
            }
            else
            {
                _secondaryWeapon = null;
            }
        }

        public void Dispose()
        {
            _parent = null;
        }
    }
}