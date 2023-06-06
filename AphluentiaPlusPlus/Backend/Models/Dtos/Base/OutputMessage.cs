namespace Backend.Models.Dtos.Base
{
    public class OutputMessage<T>
    {
        public Metadata Metadata { get; set; }
        public T Data { get; set; }

        public static OutputMessage<T> GetOutputMessage(T? obj)
        {
            return new OutputMessage<T>
            {
                Metadata = new Metadata(),
                Data = obj
            };
        }
        public static OutputMessage<T> GetOutputMessage()
        {
            return new OutputMessage<T>
            {
                Metadata = new Metadata()
            };
        }

        public OutputMessage<T> AddError(Error error)
        {
            this.Metadata.Errors.Add(error);
            return this;
        }
        
    }
}
