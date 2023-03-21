namespace Backend.Models
{
    public class Metadata
    {

        public bool Success { get; set; }
        public IList<Error> Errors { get; set; }
        public readonly DateTime? Timestamp = DateTime.UtcNow;
    }
}
