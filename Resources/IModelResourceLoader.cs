namespace Engine.Resources;

public interface IModelResourceLoader<T, TModel>
{
    public T Load(TModel model);
}