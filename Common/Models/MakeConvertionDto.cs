using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class MakeConvertionDto
    {
        [Required]
        public string Code1 {  get; set; }
        [Required]
        public string Code2 { get; set; }
        [Range(0, int.MaxValue)]
        public float Amount { get; set; }

    }
}
