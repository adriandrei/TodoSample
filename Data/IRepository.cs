using System.Linq.Expressions;
using TodoSample.Models;

namespace TodoSample.Data;

public interface IRepository<T> where T : BaseEntity
{
    T GetById(string id);
    void Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    IEnumerable<T> List(Func<KeyValuePair<string, T>, bool>? filter = null);
}
