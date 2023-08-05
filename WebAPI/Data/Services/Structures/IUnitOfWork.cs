using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data.Services.Structures
{
    public interface IUnitOfWork
    {
        ICity ICityRepo{get;}
        IUser IUserRepo{get;}
        IProperty IPropertyRepo{get;}
        IPropertyType IPropertyTypeRepo{get;}
        IFurnishingType IFurnishingTypeRepo{get;}
        Task<bool> SaveAsync();
    }
}