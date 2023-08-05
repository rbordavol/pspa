using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Data.Services.Structures
{
    public interface IPropertyType
    {
        Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
    }
}