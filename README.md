# CacheQ

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=m-khooryani_CacheQ&metric=alert_status)](https://sonarcloud.io/dashboard?id=m-khooryani_CacheQ)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/m-khooryani/CacheQ/blob/master/LICENSE)
[![CacheQ on fuget.org](https://www.fuget.org/packages/CacheQ/badge.svg)](https://www.fuget.org/packages/CacheQ)

A library for easy and convenient use of distributed caching.

## Contents

[1. Quick Start](#1-quick-start)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.1 NuGet Packages](#11-nuget-packages)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.2 Query](#12-query)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.3 Query Handler](#13-query-handler)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.4 Cache Policy](#14-cache-policy)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.5 MediatR Caching Behavior](#15-mediatr-caching-behavior)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[1.6 Dependency Injection](#16-dependency-injection)


[2. Features](#2-features)

&nbsp;&nbsp;&nbsp;[2.1 Cache Level Configuration](#21-cache-level-configuration)

&nbsp;&nbsp;&nbsp;[2.2 Cache Providers](#22-cache-providers)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[2.2.1 Memory](#221-memory)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[2.2.2 Redis](#222-redis)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[2.2.3 SQL Server](#223-sql-server)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[2.2.4 NCache](#224-ncache)

&nbsp;&nbsp;&nbsp;[2.3 Prefix Key](#23-prefix-key)

&nbsp;&nbsp;&nbsp;[2.4 Logging](#24-logging)



## 1. Quick Start

Here you can find a demonstration of how to use CacheQ in your code by following these simple steps in CQRS pattern using MediatR (personally prefer)

### 1.1 NuGet Packages

&nbsp;&nbsp;&nbsp;Required Packages:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[CacheQ](https://www.nuget.org/packages/CacheQ/)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[DI Extensions](https://www.nuget.org/packages/CacheQ.DependencyInjectionExtensions/)

&nbsp;&nbsp;&nbsp;Provider Packages:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Memory](https://www.nuget.org/packages/CacheQ.Memory/)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Redis](https://www.nuget.org/packages/CacheQ.StackExchangeRedis/)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[SQL Server](https://www.nuget.org/packages/CacheQ.SqlServer/)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[NCache](https://www.nuget.org/packages/CacheQ.NCache/)

### 1.2 Query

```csharp
public class EvenNumbersCountQuery : IRequest<int>
{
    public int StartRange { get; set; }
    public int EndRange { get; set; }
}
```

### 1.3 Query Handler

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

### 1.4 Cache Policy
In this step, you can define the cache policy by setting the ExpirationLevel and Key.
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
    
### 1.5 MediatR Caching Behavior
In this example, the use of MediatR is a must; however, it might not be necessary to use MediatR. In other words, CacheQ is not dependent on MediatR.
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

### 1.6 Dependency Injection

```csharp
services.AddMediatR(queriesAssembly);

services.AddCacheQ(queriesAssembly, 
    options =>
    {
        options.UseDistributedMemoryCache();
    });

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
```

## 2. Features

### 2.1 Cache Level Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "CacheQ": {
    "veryShort": "00:00:10",
    "short": "00:00:30",
    "regular": "00:01:00",
    "long": "00:05:00",
    "veryLong": "00:30:00"
  }
}
```


### 2.2 Cache Providers
Based on your need, you can configure the cache provider in this step. The CacheQ providers use Microsoft.Extensions.Caching internal providers.
#### 2.2.1 Memory
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[on NuGet](https://www.nuget.org/packages/CacheQ.Memory/)

```csharp
services.AddCacheQ(assembly, 
    options =>
    {
        options.UseDistributedMemoryCache(memoryCacheOptions =>
        {
            memoryCacheOptions.SizeLimit = 1024;
            // ...
        });
    });
```

#### 2.2.2 Redis
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[NuGet](https://www.nuget.org/packages/CacheQ.StackExchangeRedis/)

```csharp
services.AddCacheQ(assembly, 
    options =>
    {
        options.UseStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = "...";
            // ...
        });
    });
``` 
    
#### 2.2.3 SQL Server
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[NuGet](https://www.nuget.org/packages/CacheQ.SqlServer/)

```csharp
services.AddCacheQ(assembly, 
    options =>
    {
        options.UseDistributedSqlServerCache(sqlOptions =>
        {
            sqlOptions.ConnectionString = "...";
            sqlOptions.SchemaName = "";
            sqlOptions.TableName = "";
            // ...
        });
    });
```
    
#### 2.2.4 NCache
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[NuGet](https://www.nuget.org/packages/CacheQ.NCache/)

```csharp
services.AddCacheQ(assembly, 
    options =>
    {
        options.UseNCacheDistributedCache(nCacheOptions =>
        {
            nCacheOptions.CacheName = "democache";
            nCacheOptions.EnableLogs = true;
            nCacheOptions.ExceptionsEnabled = true;
        });
    });
```

### 2.3 Prefix Key
Consider these Queries: **EvenNumbersCountQuery** and **OddNumbersCountQuery**. Key in CachePolicy for them would be the same for a specific range(start, end) so it's needed to distinguish cache values base on Query Type (or even Query assembly)
it's customizable based on your need (**recommended as the default prefix key is quite large!**)

this is the default implementation of PrefixKey:

```csharp
builder.UsePrefixKey(type =>
    {
        return type.Assembly.GetName().Name + "," + type.FullName;
    });
```

### 2.4 Logging 
As you might have done it multiple times in other programs, Logging could be simply customized 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "CacheQ": "Information"
    }
  },
  "AllowedHosts": "*",
  "CacheQ": {
    "veryShort": "00:00:10",
    "short": "00:00:30",
    "regular": "00:01:00",
    "long": "00:05:00",
    "veryLong": "00:30:00"
  }
}
```
