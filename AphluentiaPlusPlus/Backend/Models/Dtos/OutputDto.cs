namespace Backend.Models.Dtos
{
    public class OutputDto<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
    }
}
