using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace BankTransfer.Infrastructure.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, CancellationToken token);
        Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate, CancellationToken token);
        Task<bool> Insert(T entity, CancellationToken token);
        Task<bool> Update(T entity, CancellationToken token);
        Task<bool> Delete(T entity, CancellationToken token);
    }
}