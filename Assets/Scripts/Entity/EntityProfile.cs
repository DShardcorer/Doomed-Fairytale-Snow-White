namespace Entity
{
    public class EntityProfile
    {
        private string _name = "?";
        public string Name => _name;
        public virtual void SetName(string name)
        {
            _name = name;
        }
        private string _description = "?";
        public string Description => _description;
        
        
    }
}