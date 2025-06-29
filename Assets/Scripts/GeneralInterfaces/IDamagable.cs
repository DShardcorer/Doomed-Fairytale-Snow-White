using EntityBase;

namespace GeneralInterfaces
{
    public interface IDamagable
    {
        public void TakeDamage(float damage, Entity attacker);

    }
}