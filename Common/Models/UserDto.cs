﻿using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UserDto
    {
            public string Username { get; set; }
            public string Email { get; set; }
            public SubscriptionDto Subscription { get; set; } 
            public int Conversions { get; set; }
            public UserRole Role { get; set; }
            
        

    }
}
