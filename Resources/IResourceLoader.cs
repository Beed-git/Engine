using System.Text.Json;

namespace Engine.Resources;

public interface IResourceLoader<T>
{
    public T Load(ref Utf8JsonReader reader);
}