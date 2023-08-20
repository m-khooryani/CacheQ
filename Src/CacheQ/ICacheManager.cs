namespace CacheQ;

public interface ICacheManager
{
    bool TryGetValue<TRequest, TResult>(
           ICachePolicy<TRequest> cachePolicy,
           TRequest request,
           out TResult result);

    void SetItem<TRequest, TResult>(
         ICachePolicy<TRequest> cachePolicy,
         TRequest request,
         TResult result);
}
