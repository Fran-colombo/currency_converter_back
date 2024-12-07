using Common.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Currency
    {
        
        [Key]
        [Required]
        public string Code { get; set; }
        public string Legend {  get; set; }
        public string Symbol { get; set; }
        public float ConvertionIndex { get; set; }

    }
}
