using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    public class PropertyTypeController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public PropertyTypeController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // Get api/propertytype/list
        [HttpGet("list")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var propertyTypes = await uow.IPropertyTypeRepo.GetPropertyTypesAsync();
            var propertytypesDto = mapper.Map<IEnumerable<PropertyTypeDto>>(propertyTypes);
            return Ok(propertytypesDto);
        }
    }
}