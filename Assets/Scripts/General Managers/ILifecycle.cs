public interface ILifecycle<T>
{
    public void Initialize(T parent);

    public void Dispose();
}
