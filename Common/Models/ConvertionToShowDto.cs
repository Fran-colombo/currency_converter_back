using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ConvertionToShowDto
    {
        public string Username {  get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public float Amount { get; set; }
        public float Result { get; set; }
        public DateTime Date {  get; set; }  
    }
}
