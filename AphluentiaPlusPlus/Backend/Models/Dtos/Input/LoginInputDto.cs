using SystemGateway.Dtos.Enum;

namespace Backend.Models.Dtos.Input
{
    public class LoginInputDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
