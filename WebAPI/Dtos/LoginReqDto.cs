using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class LoginReqDto
    {
        [Required]
        public required string UserName { get; set; } = "";
        [Required]
        public required string Password { get; set; } = "";
        public string? email { get; set; }
        public string? mobile { get; set; }
    }
}