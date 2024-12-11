using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Convertions
    {
        public int id {  get; set; }
        public User User { get; set; }
        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }
        public float Amount { get; set; }
        public float ConvertedAmount { get; set; }
        public DateTime Date {  get; set; }
    }
}
