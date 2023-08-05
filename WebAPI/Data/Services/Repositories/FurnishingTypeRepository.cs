using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Services.Structures;
using WebAPI.Models;

namespace WebAPI.Data.Services.Repositories
{
    public class FurnishingTypeRepository : IFurnishingType
    {
        private readonly DataContext daContext;

        public FurnishingTypeRepository(DataContext daContext)
        {
            this.daContext = daContext;
        }
        async Task<IEnumerable<FurnishingType>> IFurnishingType.GetFurnishingTypesAsync()
        {
            return await daContext.FurnishingTypes.ToListAsync();
        }
    }
}