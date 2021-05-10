# CacheQ

## Contents

[1. Introduction](#1-Introduction)

&nbsp;&nbsp;&nbsp;[1.1 What is CacheQ?](#11-what-is-cacheq)

&nbsp;&nbsp;&nbsp;[1.2 Quick Start](#12-quick-start)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.1 NuGet Packages](#121-nuget-packages)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.2 Query](#122-query)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.3 Cache Policy](#123-cache-policy)

&nbsp;&nbsp;&nbsp;[1.3 Use Cases](#12-out-of-scope)

[2. Features](#2-Domain)

&nbsp;&nbsp;&nbsp;[2.1 Description](#21-description)

## 1. Introduction

### 1.1 What is CacheQ

CacheQ assists you to implement distributed cache simply! 

### 1.2 Quick Start

#### 1.2.1 NuGet Packages

Required Packages:
CacheQ

#### 1.2.2 Query

```csharp
public class EvenNumbersCountQuery : IRequest<int>
{
    public int StartRange { get; set; }
    public int EndRange { get; set; }
}
```

#### 1.2.3
```csharp
class EvenNumbersCountQueryCachePolicy : ICachePolicy<EvenNumbersCountQuery>
{
    public CacheLevel ExpirationLevel => CacheLevel.Regular;

    public string Key(EvenNumbersCountQuery query)
    {
        return query.StartRange + "," + query.EndRange;
    }
}
```
    
MediatR caching Behaviour:

```csharp
internal class QueryCachingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> 
    where TRequest : IRequest<TResult>
{
    private readonly ICachePolicy<TRequest> _cachePolicy;
    private readonly ICacheManager _cacheManager;

    public QueryCachingBehavior(
        IEnumerable<ICachePolicy<TRequest>> cachePolicy,
        ICacheManager cacheManager)
    {
        _cachePolicy = cachePolicy.SingleOrDefault();
        _cacheManager = cacheManager;
    }

    public async Task<TResult> Handle(TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResult> next)
    {
        if (_cachePolicy is null)
        {
            return await next();
        }
        return await ReadOrUpdateCache(request, next);
    }

    private async Task<TResult> ReadOrUpdateCache(TRequest request, RequestHandlerDelegate<TResult> next)
    {
        if (_cacheManager.TryGetValue(
                        _cachePolicy,
                        request,
                        out TResult cachedResult))
        {
            return await Task.FromResult(cachedResult);
        }

        // Read From Handler
        TResult result = await next();

        // Update Cache
        _cacheManager.SetItem(_cachePolicy, request, result);

        return result;
    }
}
```

and DI

```csharp
services.AddMediatR(queriesAssembly);

services.AddCacheQ(queriesAssembly, 
    options =>
    {
        options.UseDistributedMemoryCache();
    });

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
```

now everything is set 
