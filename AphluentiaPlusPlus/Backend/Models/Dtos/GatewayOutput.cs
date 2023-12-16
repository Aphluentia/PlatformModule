namespace Backend.Models.Dtos
{
    public class GatewayOutput<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } 
        public T? Data { get; set; }
    }
}
