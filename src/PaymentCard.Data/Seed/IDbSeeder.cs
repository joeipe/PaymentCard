using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Data.Seed
{
    public interface IDbSeeder
    {
        Task SeedAsync();
    }
}
