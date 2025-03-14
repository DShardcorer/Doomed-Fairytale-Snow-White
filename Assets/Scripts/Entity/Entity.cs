

public abstract class Entity
{
    protected EntityStateMachine _stateMachine;
    protected EntityView _view;

    public EntityStateMachine StateMachine => _stateMachine;
    public EntityView View => _view;

    public Entity(EntityView view)
    {
        _view = view;
        _stateMachine = new EntityStateMachine();
    }

}
