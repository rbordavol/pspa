using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch;

namespace WebAPI.Controllers
{
    [Authorize]
    public class CityController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        //GET api/cities
        [HttpGet("cities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            var cities = await uow.ICityRepo.GetCitiesAsync();
            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            return Ok(citiesDto);
        }

        [HttpPost("post")]
        public async Task<IActionResult> AddCity(CityDto cityDto)
        {
            
            var city = mapper.Map<City>(cityDto);
            //city.UpdatedBy = "system";
            city.UpdatedOn = DateTime.Now;

            uow.ICityRepo.AddCity(city);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("delete")]
       public async Task<IActionResult> DeleteCity(int id)
        {

            uow.ICityRepo.DeleteCity(id);
            await uow.SaveAsync();

            return StatusCode(201);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDto cityDto)
        {
            if(id != cityDto.id)
                return BadRequest("Update not allowed");

            var cityFromDB = await uow.ICityRepo.GetCity(id);

            if(cityFromDB == null)
                return BadRequest("Update not allowed");
                        
            //cityFromDB.UpdatedBy = "system";
            cityFromDB.UpdatedOn = DateTime.Now;

            mapper.Map(cityDto, cityFromDB);

            //throw new Exception("Some unknown error occurred");
            
            await uow.SaveAsync();            

            return StatusCode(200);
  
        }

        //Partial updates using a partial Dto
        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityUpdateDto cityDto)
        {
            if(id != cityDto.id)
                return BadRequest("Update not allowed");

            var cityFromDB = await uow.ICityRepo.GetCity(id);

            if(cityFromDB == null)
                return BadRequest("Update not allowed");

            //cityFromDB.UpdatedBy = "system";
            cityFromDB.UpdatedOn = DateTime.Now;

            mapper.Map(cityDto, cityFromDB);
            
            await uow.SaveAsync();

            return StatusCode(200);
        }

        /* [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> cityToPatch)
        {
            var cityFromDB = await uow.ICityRepo.GetCity(id);

            if(cityFromDB != null){
                cityFromDB.UpdatedBy = "system";
                cityFromDB.UpdatedOn = DateTime.Now;

                cityToPatch.ApplyTo(cityFromDB, ModelState);
                
                await uow.SaveAsync();
            }

            return StatusCode(200);
        } */
    }
}
