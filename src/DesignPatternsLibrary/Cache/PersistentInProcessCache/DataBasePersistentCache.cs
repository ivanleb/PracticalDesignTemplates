using LiteDB;
using System;

namespace DesignPatternsLibrary.Cache.PersistentInProcessCache
{
    public class DataBasePersistentCache<TValue> : IPersistentCache<Guid, TValue>
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<CachedObject<TValue>> _collection;
        public DataBasePersistentCache(string fileName) 
        {
            _db= new LiteDatabase(fileName);
            _collection = _db.GetCollection<CachedObject<TValue>>("CachedObjects");
            _collection.EnsureIndex(x => x.Id);
        }

        public TValue Get(Guid key)
        {
            return _collection.FindOne(x => x.Id == key).Value;
        }

        public void Remove(Guid key)
        {
            _collection.DeleteMany(x => x.Id == key);
        }

        public void Set(Guid key, TValue entry)
        {
            _collection.Insert(new CachedObject<TValue>(key, entry));
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

    internal class CachedObject<TValue>
    {
        public Guid Id { get; }
        public TValue Value { get; }

        public CachedObject(Guid id, TValue value)
        {
            Id = id;
            Value = value;
        }
    }
}
