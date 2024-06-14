namespace Engine.Resources;

public interface IRawResourceLoader<T>
{
    public T Load(ResourceName name);
}