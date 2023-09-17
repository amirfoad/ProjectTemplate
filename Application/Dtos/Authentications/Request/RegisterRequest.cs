using Application.Profiles;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Authentications.Request
{
    public class RegisterRequest : ICreateMapper<Domain.Entities.Authentication.User>
    {
        [Required(ErrorMessage = "ورود نام  الزامی است")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "یک پسوورد معتبر وارد کنید.پسوورد باید حداقل 6 کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "پسوورد باید حداقل 6 کاراکتر باشد")]
        public string Password { get; set; }

        [Compare("Password")]
        public string RePassword { get; set; }
    }
}