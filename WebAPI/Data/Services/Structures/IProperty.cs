using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Data.Services.Structures
{
    public interface IProperty
    {
        Task<IEnumerable<Property>> GetPropertiesAsync(int sellRent);
        Task<Property> GetPropertyDetailsAsync(int id);
        Task<Property?> GetPropertyPhotosByIdAsync(int id);
        void AddProperty(Property property);
        void DeleteProperty(int id);
    }
}