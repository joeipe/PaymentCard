using PaymentCard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Repositories
{
    public interface ITransactionRepository :  IGenericRepository<PurchaseTransaction>
    {
    }
}
