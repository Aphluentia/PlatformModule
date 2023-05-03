using Bridge.Dtos.Entities;

namespace Bridge.Dtos.Dtos
{
    public class PollMessagesOutputDto
    {
        public bool Success { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
