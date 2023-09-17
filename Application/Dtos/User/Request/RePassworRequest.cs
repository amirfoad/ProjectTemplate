using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.User.Request
{
    public class RePassworRequest
    {
        public string PhoneNumber { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}