using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Data.Services.Structures
{
    public interface IFurnishingType
    {
        Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
    }
}