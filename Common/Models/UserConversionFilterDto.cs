using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UserConversionFilterDto
    {
        public int? Month { get; set; } // Nullable, ya que puede no ser necesario
        public int? Year { get; set; }  // Nullable para usar un valor predeterminado si es nulo
    }

}
