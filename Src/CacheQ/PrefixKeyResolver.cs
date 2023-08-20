using System;

namespace CacheQ;

public class PrefixKeyResolver
{
    public Func<Type, string> Func { get; set; }
}
