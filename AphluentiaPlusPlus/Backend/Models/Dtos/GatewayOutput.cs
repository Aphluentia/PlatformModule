namespace Backend.Models.Dtos
{
    public class GatewayOutput
    {
        public bool Success { get; set; }
        public string? Message { get; set; } 
        public object? Data { get; set; }
    }
}
