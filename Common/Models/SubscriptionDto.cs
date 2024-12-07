using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int MaxConversions { get; set; }
    }
}
