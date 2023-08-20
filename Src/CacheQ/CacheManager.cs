using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CacheQ;

internal class CacheManager : ICacheManager
{
    private readonly ISerializer _serializer;
    private readonly IDistributedCache _cache;
    private readonly ICacheExpirationResolver _cacheExpirationResolver;
    private readonly PrefixKeyResolver _prefixKeyResolver;
    private readonly ILogger<CacheManager> _logger;

    public CacheManager(
        ISerializer serializer,
        ICacheExpirationResolver cacheExpirationResolver,
        IDistributedCache cache,
        PrefixKeyResolver prefixKeyResolver,
        ILogger<CacheManager> logger)
    {
        _serializer = serializer;
        _cacheExpirationResolver = cacheExpirationResolver;
        _cache = cache;
        _prefixKeyResolver = prefixKeyResolver;
        _logger = logger;
    }

    public bool TryGetValue<TRequest, TResult>(
        ICachePolicy<TRequest> cachePolicy,
        TRequest request,
        out TResult result)
    {
        _logger.LogInformation("Checking cache...");
        var cachedData = _cache.Get(Key(cachePolicy, request));
        if (cachedData == null)
        {
            _logger.LogInformation("Key not found in cache store");
            result = default;
            return false;
        }

        result = _serializer.DeserializeFromBytes<TResult>(cachedData);
        _logger.LogInformation("Item found in cache");
        return true;
    }

    public void SetItem<TRequest, TResult>(
        ICachePolicy<TRequest> cachePolicy,
        TRequest request,
        TResult result)
    {
        var serializedData = _serializer.SerializeToBytes(result);
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = _cacheExpirationResolver
                                .GetExpiryTime(cachePolicy.ExpirationLevel),
        };
        _cache.SetAsync(
            Key(cachePolicy, request),
            serializedData,
            options);
    }

    private string Key<TRequest>(
        ICachePolicy<TRequest> cachePolicy,
        TRequest request)
    {
        _logger.LogDebug("Get Key of request");
        return _prefixKeyResolver.Func.Invoke(request.GetType()) +
            cachePolicy.Key(request);
    }
}
