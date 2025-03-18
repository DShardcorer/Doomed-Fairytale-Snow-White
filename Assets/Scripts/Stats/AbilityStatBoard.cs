
public class AbilityStatBoard
{
    public AbilityStat Strength { get; private set; }
    public AbilityStat Dexterity { get; private set; }
    public AbilityStat Constitution { get; private set; }
    public AbilityStat Intelligence { get; private set; }
    public AbilityStat Wisdom { get; private set; }
    public AbilityStat Charisma { get; private set; }

    public AbilityStatBoard(float strength = 10, float dexterity = 10, float constitution = 10, float intelligence = 10, float wisdom =10, float charisma = 10)
    {
        Strength = new AbilityStat(strength);
        Dexterity = new AbilityStat(dexterity);
        Constitution = new AbilityStat(constitution);
        Intelligence = new AbilityStat(intelligence);
        Wisdom = new AbilityStat(wisdom);
        Charisma = new AbilityStat(charisma);

    }
}
