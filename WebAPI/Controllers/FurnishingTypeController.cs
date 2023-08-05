using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    public class FurnishingTypeController : BaseController  
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public FurnishingTypeController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // Get api/furnishingtype/list
        [HttpGet("list")]
        public async Task<IActionResult> GetFurnishingTypesAsync()
        {
            var furnishingTypes = await uow.IFurnishingTypeRepo.GetFurnishingTypesAsync();
            var furnishingTypesDto = mapper.Map<IEnumerable<FurnishingTypeDto>>(furnishingTypes);

            return Ok(furnishingTypesDto);
        }
    }
}