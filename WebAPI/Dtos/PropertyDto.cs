using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class PropertyDto
    {
        
        public int SellRent { get; set; }
        public required string? PropertyName { get; set; }
        public int PropertyTypeId { get; set; }
        public int BHK { get; set; }
        public int FurnishingTypeId { get; set; }
        public int Price { get; set; }
        public int BuiltArea { get; set; }
        public int CityId { get; set; }
        public bool ReadyToMove { get; set; }
        public int CarpetArea { get; set; }
        public required string? Address { get; set; }
        public string? Address2 { get; set; }
        public int FloorNo { get; set; }
        public int TotalFloors { get; set; }
        public string? MainEntrance { get; set; }
        public int Security { get; set; } = 0;
        public bool Gated { get; set; }
        public int Maintenance { get; set; } = 0;
         public DateTime Possession { get; set; }
        public int Age { get; set; } = 0;
        public string? Description { get; set; }
    }
}