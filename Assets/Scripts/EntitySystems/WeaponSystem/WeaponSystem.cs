
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
        private WeaponView _primaryWeaponView;
        private WeaponView _secondaryWeaponView;
        
        public enum WeaponSlotType
        {
            Primary,
            Secondary
        }
        
        public void Initialize(Entity parent)
        {
            _parent = parent;
            _primaryWeaponView = _parent.View.PrimaryWeaponView;
            // if (_primaryWeaponView != null)
            // {
            //     _primaryWeapon = new Weapon();
            //     _primaryWeapon.Initialize(this, _primaryWeaponView);
            // }
            // else
            // {
            //     _primaryWeapon = null;
            // }
            _secondaryWeaponView = _parent.View.SecondaryWeaponView;
            // if (_secondaryWeaponView != null)
            // {
            //     _secondaryWeapon = new Weapon();
            //     _secondaryWeapon.Initialize(this, _secondaryWeaponView);
            // }
            // else
            // {
            //     _secondaryWeapon = null;
            // }
        }
        public void EquipWeapon(WeaponDataSO weaponData, WeaponSlotType weaponSlotType)
        {
            if (weaponData == null)
            {
                return;
            }
            Weapon newWeapon = new Weapon();
            switch (weaponSlotType)
            {
                case WeaponSlotType.Primary:
                    if (_primaryWeapon != null)
                    {
                        _primaryWeapon.Dispose();
                    }
                    _primaryWeaponView.Initialize(this, weaponData);
                    newWeapon.Initialize(this, _primaryWeaponView);
                    _primaryWeapon = newWeapon;
                    break;
                case WeaponSlotType.Secondary:
                    if (_secondaryWeapon != null)
                    {
                        _secondaryWeapon.Dispose();
                    }
                    _secondaryWeaponView.Initialize(this, weaponData);
                    newWeapon.Initialize(this,_secondaryWeaponView);
                    _secondaryWeapon = newWeapon;
                    break;
            }
        }
        public void UnequipWeapon(WeaponSlotType weaponSlotType)
        {
            switch (weaponSlotType)
            {
                case WeaponSlotType.Primary:
                    if (_primaryWeapon != null)
                    {
                        _primaryWeapon.Dispose();
                        _primaryWeapon = null;
                    }
                    break;
                case WeaponSlotType.Secondary:
                    if (_secondaryWeapon != null)
                    {
                        _secondaryWeapon.Dispose();
                        _secondaryWeapon = null;
                    }
                    break;
            }
        }

        public void Dispose()
        {
            if (_primaryWeapon != null)
            {
                _primaryWeapon.Dispose();
                _primaryWeapon = null;
            }
            if (_secondaryWeapon != null)
            {
                _secondaryWeapon.Dispose();
                _secondaryWeapon = null;
            }
            if (_primaryWeaponView != null)
            {
                _primaryWeaponView.Dispose();
                _primaryWeaponView = null;
            }
            if (_secondaryWeaponView != null)
            {
                _secondaryWeaponView.Dispose();
                _secondaryWeaponView = null;
            }
            _parent = null;
        }
        public void SetActiveWeapon(WeaponSlotType weaponSlotType)
        {
            switch (weaponSlotType)
            {
                case WeaponSlotType.Primary:
                    if (_primaryWeapon != null)
                    {
                        _primaryWeapon.SetAsActiveWeapon();
                        _secondaryWeapon?.SetAsInactiveWeapon();
                    }
                    break;
                case WeaponSlotType.Secondary:
                    if (_secondaryWeapon != null)
                    {
                        _secondaryWeapon.SetAsActiveWeapon();
                        _primaryWeapon?.SetAsInactiveWeapon();
                    }
                    break;
            }
        }
        public void SetActiveWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                return;
            }
            if (weapon == _primaryWeapon)
            {
                SetActiveWeapon(WeaponSlotType.Primary);
            }
            else if (weapon == _secondaryWeapon)
            {
                SetActiveWeapon(WeaponSlotType.Secondary);
            }
        }
    }
}