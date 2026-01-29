using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Application.Interfaces.Data
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
