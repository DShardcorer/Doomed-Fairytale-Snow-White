

public abstract class Entity
{
    protected EntityStateMachine _stateMachine;
    protected EntityView _view;
    protected EntityProperties _properties;

    public EntityStateMachine StateMachine => _stateMachine;
    public EntityView View => _view;
    public EntityProperties Properties => _properties;
    

    public Entity(EntityView view, EntityProperties properties)
    {
        _view = view;
        _properties = properties;
        _stateMachine = new EntityStateMachine();
    }
}
