﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.IRepositories
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string basketId);

        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);

        public Task<bool> DeleteBasketAsync(string basketId);
    }
}
