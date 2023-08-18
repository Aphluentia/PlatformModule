using SystemGateway.Dtos.Enum;

namespace Backend.Models.Entities
{
    public class UserDetailsDto
    {
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public string FullName { get; set; }
    }
}
