using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Services.Structures;
using WebAPI.Models;

namespace WebAPI.Data.Services.Repositories
{
    public class PropertyTypeRepository : IPropertyType

    {
        private readonly DataContext daContext;

        public PropertyTypeRepository(DataContext daContext)
        {
            this.daContext = daContext;
        }
        async Task<IEnumerable<PropertyType>> IPropertyType.GetPropertyTypesAsync()
        {
            return await daContext.PropertyTypes.ToListAsync();
        }
    }
}