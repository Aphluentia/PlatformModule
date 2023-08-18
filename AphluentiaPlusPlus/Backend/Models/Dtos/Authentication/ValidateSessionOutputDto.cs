using Backend.Models.Entities;

namespace Backend.Models.Dtos.Authentication
{
    public class ValidateSessionOutputDto
    {
        public bool IsValidSession { get; set; }
        public UserDetailsDto? UserDetails { get; set; }
        public ExpirationData SessionDetails { get; set; }  
    }
}
