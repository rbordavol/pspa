using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class User : BaseEntity
    {
        [SetsRequiredMembers]
        public User(string userName, byte[] password, byte[] passwordKey)
        =>(UserName, Password, PasswordKey) = (userName, password, passwordKey);

        [Required]
        public required string UserName { get; set; }    
        [Required]
        public required byte[] Password { get; set; }
        public required byte[] PasswordKey { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
    }
}