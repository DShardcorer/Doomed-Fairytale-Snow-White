

public enum AttackStatType { Strength, Dexterity}
public class CombatStatBoard
{
    public CombatStat Health { get; private set; }
    public CombatStat PhysicalAttack { get; private set; }
    public CombatStat MagicAttack { get; private set; }
    public CombatStat PhysicalDefense { get; private set; }
    public CombatStat MagicalDefense { get; private set; }

    public CombatStatBoard(CombatStat health, CombatStat physicalAttack, CombatStat magicAttack, CombatStat physicalDefense, CombatStat magicalDefense)
    {
        Health = health;
        PhysicalAttack = physicalAttack;
        MagicAttack = magicAttack;
        PhysicalDefense = physicalDefense;
        MagicalDefense = magicalDefense;
    }
}
