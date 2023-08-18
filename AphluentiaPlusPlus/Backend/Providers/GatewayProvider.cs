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

        public async Task<GatewayOutput> GetUserInformation(string Email)
        {
            var (success, result) = await HttpHelper.Get($"{_BaseUrl}/Authentication/Information/{Email}");
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
    }
}



