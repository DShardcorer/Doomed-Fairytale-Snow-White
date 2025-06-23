using System.Collections.Generic;

namespace EntityBase
{
    public class EntityCreationData
    {
        public string Name;


        public float Strength;
        public float Dexterity;
        public float Constitution;
        public float Intelligence;
        public float Wisdom;
        public float Charisma;

        public struct Item
        {
            public string ItemId;
            public int Quantity;
        }

        public List<Item> Items;
        public List<string> ActiveSkills;
        public List<string> PassiveSkills;

        public EntityCreationData()
        {
            Name = "DefaultName";
            Strength = 5;
            Dexterity = 5;
            Constitution = 5;
            Intelligence = 5;
            Wisdom = 5;
            Charisma = 5;

            Items = new List<Item>();
            ActiveSkills = new List<string> { "Dash" };
            PassiveSkills = new List<string> { "Natural Strength" };
        }
    }
}