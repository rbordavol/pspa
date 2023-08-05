using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class CityUpdateDto
    {
        public int id { get; set; }
        [Required]
        public required string CityName { get; set; }
    }
}