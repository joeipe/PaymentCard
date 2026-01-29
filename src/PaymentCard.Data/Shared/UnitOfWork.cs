using Microsoft.EntityFrameworkCore;
using PaymentCard.Application.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext;

        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
