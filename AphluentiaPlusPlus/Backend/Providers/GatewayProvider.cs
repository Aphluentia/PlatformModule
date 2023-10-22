using Backend.Configs;
using Backend.Helpers;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Session;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemGateway.Dtos.Enum;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;
using SystemGatewayAPI.Dtos.Entities.SecurityManager;
using ZXing.Aztec.Internal;
using static QRCoder.PayloadGenerator;

namespace Backend.Providers
{
    public class GatewayProvider: IGatewayProvider
    {
        private readonly string _BaseUrl;
        public GatewayProvider(IOptions<GatewayConfigSection> options)
        {
            _BaseUrl = options.Value.ConnectionString;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------
        // Authentication ---------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<GatewayOutput<string>> AuthenticateAndGenerateToken(UserType userType, string Email, string Password)
        {
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Authentication/Authenticate/{userType}", new AuthenticateInputDto { Email = Email, Password = Password});
            return new GatewayOutput<string> { Success = success, Data = result };
        }

      
        public async Task<GatewayOutput<string>> AuthenticationClearCachedData(string Token)
        {
            var (success, result) = await HttpHelper.Delete($"{_BaseUrl}/Authentication/{Token}/Modules");
            return new GatewayOutput<string> { Success = success, Message = result};
        }

        public async Task<GatewayOutput<SessionData>> AuthenticationFetchUserDetails(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Authentication/Information/{Token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<SessionData>() { Success = success, Message = result };
            var patient = JsonConvert.DeserializeObject<SessionData>(result);
            return new GatewayOutput<SessionData> { Success = success, Data = patient };
        }


        // ------------------------------------------------------------------------------------------------------------------------------------------
        // Patient ----------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<GatewayOutput<string>> PatientAcceptTherapist(string Token, string TherapistEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}/Accept/{TherapistEmail}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientAddNewModule(string Token, Module module)
        {
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Patient/{Token}/Modules", module);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientDeleteModule(string Token, Guid ModuleId)
        {
            var (success, result) = await HttpHelper.Delete($"{_BaseUrl}/Patient/{Token}/Modules/{ModuleId}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<ICollection<SafePatient>>> PatientFetchAll()
        {

            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ICollection<SafePatient>>() { Success = success, Message = result };
            var patient = JsonConvert.DeserializeObject<ICollection<SafePatient>>(result);
            return new GatewayOutput<ICollection<SafePatient>> { Success = success, Data = patient };
        }

        public async Task<GatewayOutput<Patient>> PatientFetchData(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Patient>() { Success = success, Message = result };
            var patient = JsonConvert.DeserializeObject<Patient>(result);
            return new GatewayOutput<Patient> { Success = success, Data = patient };
        }

        public async Task<GatewayOutput<Module>> PatientFetchModuleById(string Token, Guid ModuleId)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}/Modules/{ModuleId}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Module>() { Success = success, Message = result };
            var module = JsonConvert.DeserializeObject<Module>(result);
            return new GatewayOutput<Module> { Success = success, Data = module };
        }

        public async Task<GatewayOutput<ICollection<Module>>> PatientFetchModules(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}/Modules");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ICollection<Module>>() { Success = success, Message = result };
            var modules = JsonConvert.DeserializeObject<ICollection<Module>>(result);
            return new GatewayOutput<ICollection<Module>> { Success = success, Data = modules };
        }

        public async Task<GatewayOutput<Associations<SafeTherapist>>> PatientFetchTherapists(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}/Therapists");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Associations<SafeTherapist>>() { Success = success, Message = result };
            var associations = JsonConvert.DeserializeObject<Associations<SafeTherapist>>(result);
            return new GatewayOutput<Associations<SafeTherapist>> { Success = success, Data = associations };
        }

        public async Task<GatewayOutput<ModuleStatusCheck>> PatientModuleStatusCheck(string Token, Guid ModuleId)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Modules/{ModuleId}/Status");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ModuleStatusCheck>() { Success = success, Message = result };
            var ModuleStatusCheck = JsonConvert.DeserializeObject<ModuleStatusCheck>(result);
            return new GatewayOutput<ModuleStatusCheck> { Success = success, Data = ModuleStatusCheck };
        }

        public async Task<GatewayOutput<string>> PatientRegister(Patient patient)
        {
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Patient/Register", patient);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientRejectTherapist(string Token, string TherapistEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{Token}/Reject/{TherapistEmail}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientRemove(string Token)
        {
            var (success, result) = await HttpHelper.Delete($"{_BaseUrl}/Patient/{Token}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientUpdate(string Token, Patient patient)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Patient/{Token}", patient);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> PatientUpdateModuleToVersion(string Token, Guid ModuleId, string VersionId)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Patient/{Token}/Modules/{ModuleId}/Version/{VersionId}", "");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------
        // Therapist --------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<GatewayOutput<string>> TherapistAcceptPatient(string token, string patientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}/Accept/{patientEmail}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> TherapistDelete(string Token)
        {
            var (success, result) = await HttpHelper.Delete($"{_BaseUrl}/Therapist/{Token}");
            return new GatewayOutput<string> { Success = success, Message = result };
        }

        public async Task<GatewayOutput<ICollection<SafePatient>>> TherapistFetchAllPatients(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients/All");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ICollection<SafePatient>>() { Success = success, Message = result };
            var patient = JsonConvert.DeserializeObject<ICollection<SafePatient>>(result);
            return new GatewayOutput<ICollection<SafePatient>> { Success = success, Data = patient };
        }

        public async Task<GatewayOutput<Therapist>> TherapistFetchData(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Therapist>() { Success = success, Message = result };
            var therapist = JsonConvert.DeserializeObject<Therapist>(result);
            return new GatewayOutput<Therapist> { Success = success, Data = therapist };
        }

        public async Task<GatewayOutput<SafePatient>> TherapistFetchPatientData(string Token, string patientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients/{patientEmail}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<SafePatient>() { Success = success, Message = result };
            var patient = JsonConvert.DeserializeObject<SafePatient>(result);
            return new GatewayOutput<SafePatient> { Success = success, Data = patient };
        }
 
        public async Task<GatewayOutput<Associations<SafePatient>>> TherapistFetchPatients(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Associations<SafePatient>> () { Success = success, Message = result };
            var patientList = JsonConvert.DeserializeObject<Associations<SafePatient>> (result);
            return new GatewayOutput<Associations<SafePatient>> { Success = success, Data = patientList };
        }

        public async Task<GatewayOutput<Module>> TherapistFetchPatientsModuleById(string Token, string patientEmail, Guid ModuleId)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients/{patientEmail}/Modules/{ModuleId}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<Module>() { Success = success, Message = result };
            var module = JsonConvert.DeserializeObject<Module>(result);
            return new GatewayOutput<Module> { Success = success, Data = module };
        }

        public async Task<GatewayOutput<ICollection<Module>>> TherapistFetchPatientsModules(string Token, string patientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients/{patientEmail}/Modules");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ICollection<Module>>() { Success = success, Message = result };
            var module = JsonConvert.DeserializeObject<ICollection<Module>>(result);
            return new GatewayOutput<ICollection<Module>> { Success = success, Data = module };
        }

        public async Task<GatewayOutput<ModuleStatusCheck>> TherapistModuleStatusCheck(string Token, string PatientEmail, Guid ModuleId)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{Token}/Patients/{PatientEmail}/Modules/{ModuleId}/Status");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ModuleStatusCheck>() { Success = success, Message = result };
            var module = JsonConvert.DeserializeObject<ModuleStatusCheck>(result);
            return new GatewayOutput<ModuleStatusCheck> { Success = success, Data = module };
        }

        public async Task<GatewayOutput<string>> TherapistRegister(Therapist therapist)
        {
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Therapist/Register", therapist);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> TherapistRejectPatient(string token, string patientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}/Reject/{patientEmail}");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<ICollection<SafeTherapist>>> TherapistsFetchAll()
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput<ICollection<SafeTherapist>> () { Success = success, Message = result };
            var module = JsonConvert.DeserializeObject< ICollection<SafeTherapist>> (result);
            return new GatewayOutput<ICollection<SafeTherapist>> { Success = success, Data = module };
        }

        public async Task<GatewayOutput<string>> TherapistUpdate(string Token, Therapist data)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Therapist/{Token}", data);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> TherapistUpdatePatientModule(string Token, string PatientEmail, Guid ModuleId, Module module)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Therapist/{Token}/Patients/{PatientEmail}/Modules/{ModuleId}", module);
            return new GatewayOutput<string>() { Success = success, Message = result };
        }

        public async Task<GatewayOutput<string>> TherapistUpdatePatientModuleToVersion(string Token, string PatientEmail, Guid ModuleId, string VersionId)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Therapist/{Token}/Patient/{PatientEmail}/Modules/{ModuleId}/Version/{VersionId}","");
            return new GatewayOutput<string>() { Success = success, Message = result };
        }
    }
}



