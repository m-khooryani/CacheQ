namespace CacheQ
{
    public class CacheValueModel<T>
    {
        public T Item { get; }

        public CacheValueModel(T item)
        {
            Item = item;
        }
    }
}
