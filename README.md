# CacheQ

## Contents

[1. Introduction](#1-Introduction)

&nbsp;&nbsp;&nbsp;[1.1 What is CacheQ?](#11-what-is-cacheq)

&nbsp;&nbsp;&nbsp;[1.2 Quick Start](#12-quick-start)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.1 NuGet Packages](#121-nuget-packages)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.2 Query](#122-query)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.3 Query Handler](#123-query-handler)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.4 Cache Policy](#124-cache-policy)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.5 MediatR Caching Behavior](#125-mediatr-caching-behavior)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2.6 Dependency Injection](#126-dependency-injection)

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

#### 1.2.3 Query Handler

```csharp
class EvenNumbersCountQueryHandler : IRequestHandler<EvenNumbersCountQuery, int>
{
    public Task<int> Handle(EvenNumbersCountQuery request, CancellationToken cancellationToken)
    {
        int count = 0;
        for (int i = request.StartRange; i < request.EndRange; i++)
        {
            if (i % 2 == 0)
            {
                count++;
            }
        }
        return Task.FromResult(count);
    }
}
```

#### 1.2.3 Cache Policy
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
    
#### 1.2.4 MediatR Caching Behavior

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

#### 1.2.5 Dependency Injection

```csharp
services.AddMediatR(queriesAssembly);

services.AddCacheQ(queriesAssembly, 
    options =>
    {
        options.UseDistributedMemoryCache();
    });

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
```

