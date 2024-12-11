using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string confirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        public int SubscriptionId {  get; set; }
        [ForeignKey("SubscriptionId")]
        public Subscription Subscription { get; set; }
        public int conversions { get; set; } 
        public UserRole Role { get; set; }
        //public bool? isDeleted { get; set; } = false;
    }
}
