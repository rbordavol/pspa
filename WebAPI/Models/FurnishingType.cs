using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class FurnishingType : BaseEntity
    {
        [Required]
        public required string? FurnishingTypeName { get; set; }
    }
}