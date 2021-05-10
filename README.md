# CacheQ

## Contents

[1. Introduction](#1-Introduction)

&nbsp;&nbsp;[1.1 What is CacheQ?](#11-what-is-cacheq)

&nbsp;&nbsp;[1.2 Quick Start](#11-purpose-of-this-repository)

&nbsp;&nbsp;[1.3 Use Cases](#12-out-of-scope)

[2. Features](#2-Domain)

&nbsp;&nbsp;[2.1 Description](#21-description)

## 1. Introduction

### 1.1 What is CacheQ

CacheQ assists you to implement distributed cache simply! 

### 1.2 Quick Start

This is a simple sample usage with the CQRS pattern and MediatR:

first define a query

Required Packages:

```csharp
public class EvenNumbersCountQuery : IRequest<int>
{
    public int StartRange { get; set; }
    public int EndRange { get; set; }
}
```

then cache policy
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
