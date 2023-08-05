using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }        
        public DateTime UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}