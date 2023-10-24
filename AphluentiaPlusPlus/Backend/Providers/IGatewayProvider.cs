using Backend.Models;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Session;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SystemGateway.Dtos.Enum;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;

namespace Backend.Providers
{
    public interface IGatewayProvider
    {
        // Authentication Endpoints
        public Task<GatewayOutput<string>> AuthenticateAndGenerateToken(UserType userType, string Email, string Password);
        public Task<GatewayOutput<SessionData>> AuthenticationFetchUserDetails(string token);
        public Task<GatewayOutput<string>> AuthenticationClearCachedData(string token);


        // Patient Endpoints ------------------------------------------------------------------------------------------------------------------------
        public Task<GatewayOutput<string>> PatientRegister(Patient patient);
        public Task<GatewayOutput<string>> PatientRemove(string Token);
        public Task<GatewayOutput<string>> PatientUpdate(string token, Patient therapist);
        public Task<GatewayOutput<string>> PatientAddNewModule(string Token, Module module);
        public Task<GatewayOutput<string>> PatientUpdateModuleToVersion(string Token, Guid ModuleId, string VersionId);
        public Task<GatewayOutput<string>> PatientDeleteModule(string Token, Guid ModuleId);
        public Task<GatewayOutput<string>> PatientRejectTherapist(string Token, string TherapistEmail);
        public Task<GatewayOutput<string>> PatientAcceptTherapist(string Token, string TherapistEmail);
        public Task<GatewayOutput<Patient>> PatientFetchData(string Token);
        public Task<GatewayOutput<ICollection<Module>>> PatientFetchModules(string Token);
        public Task<GatewayOutput<Module>> PatientFetchModuleById(string Token, Guid ModuleId);
        public Task<GatewayOutput<ICollection<SafePatient>>> PatientFetchAll();
        public Task<GatewayOutput<ModuleStatusCheck>> PatientModuleStatusCheck(string Token, Guid ModuleId);
        public Task<GatewayOutput<Associations<SafeTherapist>>> PatientFetchTherapists(string Token);



        // Therapist Endpoints ----------------------------------------------------------------------------------------------------------------------
        public Task<GatewayOutput<string>> TherapistRegister(Therapist therapist);
        public Task<GatewayOutput<string>> TherapistUpdate(string Token, Therapist data);
        public Task<GatewayOutput<string>> TherapistDelete(string Token);
        public Task<GatewayOutput<string>> TherapistRejectPatient(string token, string patientEmail);
        public Task<GatewayOutput<string>> TherapistAcceptPatient(string token, string patientEmail);
        public Task<GatewayOutput<Therapist>> TherapistFetchData(string Token);
        public Task<GatewayOutput<ICollection<SafeTherapist>>> TherapistsFetchAll();
        public Task<GatewayOutput<Associations<SafePatient>>> TherapistFetchPatients(string Token);
        public Task<GatewayOutput<SafePatient>> TherapistFetchPatientData(string Token, string patientEmail);
        public Task<GatewayOutput<ICollection<Module>>> TherapistFetchPatientsModules(string Token, string patientEmail);
        public Task<GatewayOutput<Module>> TherapistFetchPatientsModuleById(string Token, string patientEmail, Guid ModuleId);
        public Task<GatewayOutput<string>> TherapistUpdatePatientModule(string Token, string PatientEmail, Guid ModuleId, Module module);
        public Task<GatewayOutput<string>> TherapistUpdatePatientModuleToVersion(string Token, string PatientEmail, Guid ModuleId, string VersionId);
        public Task<GatewayOutput<ModuleStatusCheck>> TherapistModuleStatusCheck(string Token, string PatientEmail, Guid ModuleId);
        public Task<GatewayOutput<ICollection<SafePatient>>> TherapistFetchAllPatients(string Token);

        public Task<GatewayOutput<ICollection<Application>>> ApplicationFetchAll();

    }
}
