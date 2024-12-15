using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CurrencyForDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Legend { get; set; }
        public string? Symbol { get; set; }
        [Required]
        public float ConvertionIndex { get; set; }
        //public bool isActive { get; set; } = true;

    }
}
