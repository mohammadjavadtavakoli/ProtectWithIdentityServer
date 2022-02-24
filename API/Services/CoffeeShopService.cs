using API.Models;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class CoffeeShopService : ICoffeeShopService
    {
        private readonly ApplicationDbContext _DbContext;

        public CoffeeShopService(ApplicationDbContext DbContext)
        {
               _DbContext = DbContext;
        }

        public async Task<List<CoffeeShopModel>> List()
        {
            var cofeeshops = await (from shop in _DbContext.CoffeeShop select 
                                   new CoffeeShopModel()
                                   {
                                       Id = shop.Id,
                                       Name = shop.Name,
                                       OpeningHours = shop.OpeningHours,
                                       Address = shop.Address
                                   }).ToListAsync();
            return cofeeshops;

        }
    }
}
 