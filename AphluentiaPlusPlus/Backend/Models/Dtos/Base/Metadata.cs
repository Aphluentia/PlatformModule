﻿namespace Backend.Models.Dtos.Base
{
    public class Metadata
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public bool Success => !Errors.Any();
        public ICollection<Error> Errors { get; set; } = new List<Error>();
        public DateTime ActionTimestamp => DateTime.UtcNow;
    }
}
