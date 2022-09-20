using System.Threading.Tasks;

namespace Chaos.Services.Caches.Abstractions;

public interface ISimpleCache<out TResult> : IEnumerable<TResult>
{
    TResult GetObject(string key);

    Task ReloadAsync();
}

public interface ISimpleCache
{
    TResult GetObject<TResult>(string key);
}

public interface ISimpleCacheProvider
{
    ISimpleCache<T> GetCache<T>();
}