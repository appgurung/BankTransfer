using BankTransfer.Infrastructure.Context;
using BankTransfer.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Infrastructure.Services
{
    public class RepositoryService<T> : IRepository<T> where T : class
    {
        private readonly CoreBankingContext _context;
        public RepositoryService(CoreBankingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate, int pageNo, int pageSize, CancellationToken token)
        {
            return _context.Set<T>().Where(predicate)
                    .Skip((pageNo - 1) * pageSize).AsQueryable();
        }

        public async Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate, token);
        }
        public async Task<bool> Insert(T entity, CancellationToken ct = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }
        public async Task<bool> Update(T entity, CancellationToken token)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync(token) > 0;
        }
        public async Task<bool> Delete(T entity, CancellationToken token)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync(token) > 0;
        }
    }
}
