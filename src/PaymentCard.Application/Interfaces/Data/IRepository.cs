using PaymentCard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Application.Interfaces.Data
{
    public interface IRepository<T>
    {
        Task<T?> FindAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        void Create(params IEnumerable<T> items);

        void Update(params IEnumerable<T> items);

        void Delete(params IEnumerable<T> items);
    }
}
