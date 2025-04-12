using Entity.Faction;

namespace Entity.Player
{
    public class PlayerProperties : EntityProperties
    {
        public PlayerProperties(EntityFaction entityFaction, float maxHealth) : base(entityFaction, maxHealth)
        {
        }
    }
}
