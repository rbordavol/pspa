using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class CityDto
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Name is mandatory field")]
        [StringLength(200, MinimumLength = 2)]
        //[RegularExpression(".*[a-zA-Z]+.*", ErrorMessage="No numerics are allowed")]
        public required string CityName { get; set; }
        [Required]
        public required string Country { get; set; }
    }
}