public class GlobalConfigurationCache
{
    private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    //Dictionary is not thread safe
    //Solve this problem using Concurrent Data Structures or ConcurrentDictionary
    private Dictionary<int, string> _cache = new Dictionary<int, string>();
    public void Add(int key, string value)
    {
        bool lockAcquired = false;
        try
        {
            //Exclusive lock, everyone else is blocked
            _lock.EnterWriteLock();
            lockAcquired = true;
            //Not an atomic operation, broken into multiple parts while being executed.
            _cache[key] = value;
        }
        finally
        {
            if (lockAcquired)
            {
                _lock.ExitWriteLock();
            }
        }
    }
    public string? Get(int key)
    {
        bool lockAcquired = false;
        try
        {
            //Allow different reader threads to access cache simultaneously
            _lock.EnterReadLock(); 
            lockAcquired = true;
            //not an atomic operation
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
        finally 
        {
            if (lockAcquired)
            {
                _lock.ExitReadLock();
            }
        }
    }
}
