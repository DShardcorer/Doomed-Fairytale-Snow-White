using System.Collections.Generic;
using System.Linq;
using EntityBase;
using EntitySystems.WeaponSystem.Components.ComponentData;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    [CreateAssetMenu(fileName = "WeaponDataSO", menuName = "DataSO/WeaponDataSO/BasicWeaponData", order = 1)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public List<BodyTypeSpecificData> BodyTypeSpecificData { get; private set; }
        [field: SerializeField] private RuntimeAnimatorController DefaultWeaponAnimatorController { get; set; }
        [field: SerializeReference] private WeaponHitboxData DefaultWeaponHitbox { get; set; }

        [field: SerializeReference] public List<WeaponComponentData> ComponentDataList { get; private set; }

        public T GetComponentData<T>() where T : WeaponComponentData
        {
            return ComponentDataList.OfType<T>().FirstOrDefault();
        }

        public RuntimeAnimatorController GetBodyTypeAnimatorController(BodyType bodyType)
        {
            //try finding, if not found, return the first one
            var specificData = BodyTypeSpecificData.FirstOrDefault(data => data.BodyType == bodyType);
            if (specificData != null)
            {
                return specificData.WeaponAnimatorController;
            }
            else
            {
                Debug.Log($"No specific animator controller found for body type {bodyType}. Using default.");
                return DefaultWeaponAnimatorController;
            }
        }
        public WeaponHitboxData GetBodyTypeHitboxData(BodyType bodyType)
        {
            //try finding, if not found, return the first one
            var specificData = BodyTypeSpecificData.FirstOrDefault(data => data.BodyType == bodyType);
            if (specificData != null)
            {
                return specificData.WeaponHitbox;
            }
            else
            {
                Debug.Log($"No specific hitbox data found for body type {bodyType}. Using default.");
                return DefaultWeaponHitbox;
            }
        }
    }
}