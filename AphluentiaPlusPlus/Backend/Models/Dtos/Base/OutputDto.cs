namespace Backend.Models.Dtos.Base
{
    public class OutputDto
    {
        public Metadata Metadata { get; set; }
        public object? Data { get; set; }
        public OutputDto AddError(string error)

        {
            Metadata.Success = false;
            Metadata.Errors.Add(error);
            return this;

        }
        public OutputDto AddError(Error error)
        {
            Metadata.Success = false;
            
            Metadata.Errors.Add($"{error.Code}: {error.Message}");
            return this;

        }
    }
}
