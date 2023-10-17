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




        // Therapist Endpoints
        public Task<GatewayOutput> GetTherapistData(string email, string token);
        public Task<GatewayOutput> FetchAllTherapists(string token);
        public Task<GatewayOutput> UpdateTherapistData(string token, Therapist therapist);
        public Task<GatewayOutput> TherapistRejectPatient(string token, string PatientEmail);
        public Task<GatewayOutput> TherapistAcceptPatient(string token, string PatientEmail);
        public Task<GatewayOutput> FetchTherapistPatients(string token);

        // Patient Endpoints 

        public Task<GatewayOutput> FetchPatientData(string email, string token);
        public Task<GatewayOutput> FetchAllPatients(string token);
        public Task<GatewayOutput> UpdatePatientData(string token, Patient patient);
        public Task<GatewayOutput> PatientRejectTherapist(string token, string TherapistEmail);
        public Task<GatewayOutput> PatientAcceptTherapist(string token, string TherapistEmail);
        public Task<GatewayOutput> FetchPatientTherapist(string token);
    }
}
