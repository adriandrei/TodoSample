using System.Collections.Concurrent;
using TodoSample.Models;

namespace TodoSample.Data;

public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    private ConcurrentDictionary<string, T> _database = new ConcurrentDictionary<string, T>();

    public void Create(T entity)
    {
        if (_database.ContainsKey(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} already exists");

        _database.TryAdd(entity.Id, entity);
    }

    public void Delete(T entity)
    {
        if (!_database.ContainsKey(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} doesn't exist");

        _database.TryRemove(entity.Id, out _);
    }

    public T GetById(string id)
    {
        _database.TryGetValue(id, out T value);
        return value ;
    }

    public IEnumerable<T> List(Func<KeyValuePair<string, T>, bool>? filter = null)
    {
        var result = filter != null ?
            _database.Where(filter).Select(t => t.Value) :
            _database.Select(t => t.Value);

        return result.OrderBy(t => t.Updated);
    }

    public T Update(T entity)
    {
        if (!_database.ContainsKey(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} doesn't exist");

        entity.Updated = DateTime.UtcNow;
        _database[entity.Id] = entity;

        return entity;
    }
}
