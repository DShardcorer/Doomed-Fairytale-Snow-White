namespace Entity
{
    public class EntityProfile
    {
        private string _name = "?";
        public string Name => _name;
        public void SetName(string name)
        {
            _name = name;
        }
        
        
    }
}