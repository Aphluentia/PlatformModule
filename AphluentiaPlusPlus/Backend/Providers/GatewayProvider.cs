using Backend.Configs;
using Backend.Helpers;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Authentication;
using Backend.Models.Entities;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using SystemGateway.Dtos.Enum;
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

        public async Task<GatewayOutput> Authenticate(LoginInputDto dto)
        {
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Authentication/AuthenticateUser", dto);
            return new GatewayOutput { Success = success, Message = result };  
        }

        
        public async Task<GatewayOutput> FetchAllTherapists(string token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var patientList = JsonConvert.DeserializeObject<List<Patient>>(result);
            return new GatewayOutput { Success = success, Data = patientList };
        }
        public async Task<GatewayOutput> FetchAllPatients(string token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var patientList = JsonConvert.DeserializeObject<List<Patient>>(result);
            return new GatewayOutput { Success = success, Data = patientList };
        }


     

        public async Task<GatewayOutput> GetTherapistData(string email, string token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{email}/{token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var userDetails = JsonConvert.DeserializeObject<Therapist>(result);
            return new GatewayOutput { Success = success, Data = userDetails };
        }

        public async Task<GatewayOutput> GetUserInformation(string Token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Authentication/Information/{Token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var userDetails = JsonConvert.DeserializeObject<UserDetailsDto>(result);
            return new GatewayOutput { Success = success, Data = userDetails };
        }

        public async Task<GatewayOutput> RegisterPatient(Patient patient)
        {
            if (string.IsNullOrEmpty(patient.PhoneNumber)) patient.PhoneNumber = "Empty Phone Number";
            if (string.IsNullOrEmpty(patient.CountryCode)) patient.CountryCode = "+000";
            if (string.IsNullOrEmpty(patient.ConditionName)) patient.ConditionName = "Condition";
            if (patient.ConditionAcquisitionDate == DateTime.MinValue || patient.ConditionAcquisitionDate == null ) patient.ConditionAcquisitionDate = DateTime.UtcNow;
            if (string.IsNullOrEmpty(patient.ProfilePicture)) patient.ProfilePicture = "Profile Picture";
            patient.AcceptedTherapists = new HashSet<string>();
            patient.RequestedTherapists = new HashSet<string>();
            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Authentication/RegisterPatient", patient);
            return new GatewayOutput { Success = success, Message = result };
        }

        public async Task<GatewayOutput> RegisterTherapist(Therapist therapist)
        {
            if (string.IsNullOrEmpty(therapist.Credentials)) therapist.Credentials = "No information is available";
            if (string.IsNullOrEmpty(therapist.Description)) therapist.Description = "No information is available";
            if (string.IsNullOrEmpty(therapist.ProfilePicture)) therapist.ProfilePicture = "Profile Picture";
            therapist.PatientsAccepted = new HashSet<string>();
            therapist.PatientRequests = new HashSet<string>();

            var (success, result) = await HttpHelper.Post($"{_BaseUrl}/Authentication/RegisterTherapist", therapist);
            return new GatewayOutput { Success = success, Message = result };
        }

        public async Task<GatewayOutput> TherapistAcceptPatient(string token, string PatientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}/Accept/{PatientEmail}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            return new GatewayOutput { Success = success, Data = null };
        }

        public async Task<GatewayOutput> TherapistRejectPatient(string token, string PatientEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}/Reject/{PatientEmail}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            return new GatewayOutput { Success = success, Data = null };
        }

        public async Task<GatewayOutput> UpdateTherapistData(string token, Therapist therapist)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Therapist/{token}", therapist);
            return new GatewayOutput { Success = success, Message = result };
        }


        public async Task<GatewayOutput> FetchTherapistPatients(string token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Therapist/{token}/Patients");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var patientList = JsonConvert.DeserializeObject<TherapistPatients>(result);
            return new GatewayOutput { Success = success, Data = patientList };
        }

        async Task<GatewayOutput> IGatewayProvider.FetchPatientData(string email, string token)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{email}/{token}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var userDetails = JsonConvert.DeserializeObject<Patient>(result);
            return new GatewayOutput { Success = success, Data = userDetails };
        }

        async Task<GatewayOutput> IGatewayProvider.UpdatePatientData(string token, Patient patient)
        {
            var (success, result) = await HttpHelper.Put($"{_BaseUrl}/Patient/{token}", patient);
            return new GatewayOutput { Success = success, Message = result };
        }

        async Task<GatewayOutput> IGatewayProvider.PatientRejectTherapist(string token, string TherapistEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{token}/Reject/{TherapistEmail}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            return new GatewayOutput { Success = success, Data = null };
        }

        async Task<GatewayOutput> IGatewayProvider.PatientAcceptTherapist(string token, string TherapistEmail)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{token}/Accept/{TherapistEmail}");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            return new GatewayOutput { Success = success, Data = null };
        }

        async Task<GatewayOutput> IGatewayProvider.FetchPatientTherapist(string token)
        {

            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Patient/{token}/Therapist");
            if (string.IsNullOrEmpty(result) || !success) return new GatewayOutput() { Success = success, Message = result };
            var therapistList = JsonConvert.DeserializeObject<PatientTherapist>(result);
            return new GatewayOutput { Success = success, Data = therapistList };
        }
    }
}



