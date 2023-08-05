using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PropertyType : BaseEntity
    {
        [Required]
        public required string? PropertyTypeName { get; set; }
    }
}