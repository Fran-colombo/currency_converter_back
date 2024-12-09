using Common.Enum;
using System.ComponentModel.DataAnnotations;


namespace Common.Models
{
    public class UserForCreationDto
    {
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "You must use, at least, 8 characters for your password")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string confirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public SubscriptionType Subscription { get; set; }

    }
}
