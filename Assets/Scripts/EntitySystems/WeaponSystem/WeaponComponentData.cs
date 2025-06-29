using System;

namespace EntitySystems.WeaponSystem
{
    [System.Serializable]
    public abstract class WeaponComponentData
    {
        public Type DependencyType { get; protected set; }
    }
}