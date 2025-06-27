using System.Collections.Generic;
using System.Linq;
using EntitySystems.WeaponSystem.Components.ComponentData;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    [CreateAssetMenu(fileName = "WeaponDataSO", menuName = "DataSO/WeaponDataSO/BasicWeaponData", order = 1)]
    public class WeaponDataSO: ScriptableObject
    {
        [field:SerializeField] public RuntimeAnimatorController WeaponAnimatorController { get; private set; }
        
        [field:SerializeField] public List<WeaponComponentData> ComponentDataList { get; private set; }
        
        public T GetComponentData<T>() where T : WeaponComponentData
        {
            return ComponentDataList.OfType<T>().FirstOrDefault();
        }
        
        [ContextMenu("Add Movement Component")]
        public void AddMovementComponent()
        {
            if (GetComponentData<WeaponMovementData>() == null)
            {
                ComponentDataList.Add(new WeaponMovementData());
            }
            else
            {
                Debug.LogWarning("WeaponMovementData already exists in the list.");
            }
        }
    }
}