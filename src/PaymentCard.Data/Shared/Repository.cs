using Microsoft.EntityFrameworkCore;
using PaymentCard.Application.Interfaces.Data;
using PaymentCard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Shared
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _database;
        private readonly DbSet<T> _dataTable;

        public Repository(DbContext database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(DbContext));
            _dataTable = _database.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataTable.ToListAsync();
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _dataTable.SingleOrDefaultAsync(e => e.Id == id);
        }

        public void Create(params IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _database.Add(item);
            }
        }

        public void Update(params IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _database.Update(item);
            }
        }

        public void Delete(params IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _database.Remove(item);
            }
        }
    }
}
