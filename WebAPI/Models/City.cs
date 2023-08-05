using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class City : BaseEntity
    {
        [Required]
        public required string? CityName { get; set; } = null;
        [Required]
        public required string? Country { get; set; } = null;
    }
}