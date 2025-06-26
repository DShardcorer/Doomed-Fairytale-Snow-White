using System;
using EntityBase;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components
{
    public class WeaponSprite: WeaponComponent
    {
        private SpriteRenderer _entitySpriteRenderer;
        private SpriteRenderer _weaponSpriteRenderer;
        private Entity _entity;

        [SerializeField] private WeaponSpritesForOneAttack[] weaponSprites;
        
        private int _currentSpriteIndex = 0;
        public override void Initialize(Weapon parent)
        {
            base.Initialize(parent);
            _entity = _weapon.Parent.Parent;
            _entitySpriteRenderer = _weapon.Parent.Parent.View.SpriteRenderer;
            _weaponSpriteRenderer = _weapon.View.WeaponSpriteRenderer;
            _entitySpriteRenderer.RegisterSpriteChangeCallback(HandleEntitySpriteChange);
            _entity.OnAttackStarts += ResetSpriteIndex;
        }

        private void ResetSpriteIndex()
        {
            _currentSpriteIndex = 0;
        }

        private void HandleEntitySpriteChange(SpriteRenderer sr)
        {
            if (!_entity.IsAttacking())
            {
                return;
            }
            _weaponSpriteRenderer.sprite = weaponSprites[_entity.CurrentAttackCounter()].Sprites[_currentSpriteIndex];
            _currentSpriteIndex++;
            
        }

        public override void Dispose()
        {
            base.Dispose();
            _weaponSpriteRenderer.UnregisterSpriteChangeCallback(HandleEntitySpriteChange);
            _entity = null;
            _entitySpriteRenderer = null;
            _weaponSpriteRenderer = null;

        }
        
        
    }
    [Serializable]
    public class WeaponSpritesForOneAttack
    {
        [field:SerializeField] public Sprite[] Sprites {get; private set;}
    }
}