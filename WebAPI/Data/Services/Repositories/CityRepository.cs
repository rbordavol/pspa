using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Services.Structures;
using WebAPI.Models;

namespace WebAPI.Data.Services.Repositories
{
    public class CityRepository : ICity
    {
        private readonly DataContext daContext;

        public CityRepository(DataContext daContext)
        {
            this.daContext = daContext;
        }
        void ICity.AddCity(City city)
        {
            daContext.Cities.AddAsync(city);
        }

        void ICity.DeleteCity(int id)
        {
            var city = daContext.Cities.Find(id);

            if(city != null){
                daContext.Cities.Remove(city);             
            }             
        }

        async Task<IEnumerable<City>> ICity.GetCitiesAsync()
        {
            var cities = await daContext.Cities.OrderBy(x => x.CityName).ToListAsync();
            return cities;
        }

        async Task<City?> ICity.GetCity(int id)
        {
            return await daContext.Cities.FindAsync(id);
        }
    }
}