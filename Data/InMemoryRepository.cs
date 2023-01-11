#region

using System.Collections.Concurrent;
using TodoSample.Models;

#endregion

namespace TodoSample.Data;

public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ConcurrentDictionary<string, T> _database = new();

    public void Create(T entity)
    {
        if (Exists(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} already exists");

        _database.TryAdd(entity.Id, entity);
    }

    public void Delete(T entity)
    {
        if (!Exists(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} doesn't exist");

        _database.TryRemove(entity.Id, out _);
    }

    public bool Exists(string id)
    {
        return _database.ContainsKey(id);
    }

    public T GetById(string id)
    {
        _database.TryGetValue(id, out var value);
        return value;
    }

    public IEnumerable<T> List(Func<KeyValuePair<string, T>, bool>? filter = null)
    {
        var result = filter != null ? _database.Where(filter).Select(t => t.Value) : _database.Select(t => t.Value);

        return result.OrderBy(t => t.Updated);
    }

    public T Update(T entity)
    {
        if (!Exists(entity.Id))
            throw new Exception($"Entity with Id {entity.Id} doesn't exist");

        entity.Updated = DateTime.UtcNow;
        _database[entity.Id] = entity;

        return entity;
    }
}