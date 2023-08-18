using Backend.Models.Dtos;
using Backend.Models.Dtos.Authentication;
using Backend.Models.Entities;
using DatabaseApi.Models.Entities;
using SystemGateway.Dtos.Enum;

namespace Backend.Providers
{
    public interface IGatewayProvider
    {
        public Task<GatewayOutput> RegisterPatient(Patient patient);
        public Task<GatewayOutput> RegisterTherapist(Therapist therapist);
        public Task<GatewayOutput> Authenticate(LoginInputDto dto);
        public Task<GatewayOutput> GetUserInformation(string Email);
    }
}
