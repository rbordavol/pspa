using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebAPI.Data.Services.Repositories;
using WebAPI.Data.Services.Structures;

namespace WebAPI.Data.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext daContext;
        private readonly IOptions<AWSOptions> options;
        private readonly IPhoto iphoto;

        public UnitOfWork(DataContext daContext, IOptions<AWSOptions> options, IPhoto iphoto)
        {
            this.daContext = daContext;
            this.options = options;
            this.iphoto = iphoto;
        }
        ICity IUnitOfWork.ICityRepo => new CityRepository(daContext);

        IUser IUnitOfWork.IUserRepo => new UserRepository(daContext);

        IProperty IUnitOfWork.IPropertyRepo => new PropertyRepository(daContext, options, iphoto);
        
        IPropertyType IUnitOfWork.IPropertyTypeRepo => new PropertyTypeRepository(daContext);
        IFurnishingType IUnitOfWork.IFurnishingTypeRepo => new FurnishingTypeRepository(daContext);

        async Task<bool> IUnitOfWork.SaveAsync()
        {
            return await daContext.SaveChangesAsync() > 0;
        }
    }
}