namespace CacheQ
{
    public interface ICachePolicy<T>
    {
        string Key(T query);
        CacheLevel ExpirationLevel { get; }
    }
}
