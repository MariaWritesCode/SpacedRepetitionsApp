using Microsoft.EntityFrameworkCore;
using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepApp.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository where T : class
    {
        private protected DbContext _context;
        private protected ICacheService _cacheService;
        private protected readonly string cacheKey = $"{typeof(T)}";
        private protected const string AllTagsKey = "Tags:All";

        public GenericRepository(DbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;            
        }

        
        public virtual T Add(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsync(T t)
        {
            _context.Set<T>().Add(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public virtual void Delete(T t, object key)
        {
            if (t == null)
            {
                return;
            }

            T exists = _context.Set<T>().Find(key);

            if (exists != null)
            {
                _context.Remove(exists);
                _context.SaveChanges();
            }
        }

        public virtual async Task DeleteAsync(object key)
        {            
            T exists = await _context.Set<T>().FindAsync(key).ConfigureAwait(false);

            if (exists == null)
            {
                return;
            }

            if (exists is Tag)
            {
                await _cacheService.Remove($"{cacheKey}:{(exists as Tag).Name}");
            }

            _context.Remove(exists);
            await _context.SaveChangesAsync();
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
            {
                return null;
            }

            T exist = _context.Set<T>().Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                _context.SaveChanges();
            }

            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
            {
                return null;
            }

            T exist = await _context.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                await _context.SaveChangesAsync();
            }

            return exist;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        public async Task RefreshCache()
        {
            await _cacheService.Remove(cacheKey);
            var cachedList = await _context.Set<T>().ToListAsync();
            await _cacheService.Set(cacheKey, cachedList);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

