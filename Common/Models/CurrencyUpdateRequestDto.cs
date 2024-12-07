using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CurrencyUpdateRequestDto
    {
        
        public string code { get; set; }
        
        public float convertionIndex { get; set; }
    }
}
