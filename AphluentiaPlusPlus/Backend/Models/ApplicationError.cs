namespace Backend.Models
{
    public class ApplicationError
    {
        public static Error SessionIdRequired => new Error(nameof(SessionIdRequired), "Session ID is required");

        public static Error SessionIsNotAvailable => new Error(nameof(SessionIsNotAvailable), "No Session With Provided SessionID Was Found");
        public static Error SessionNotValid => new Error(nameof(SessionNotValid), "The Provided Session is not Valid");
        public static Error SessionExpired => new Error(nameof(SessionExpired), "The Provided Session is has expired");
        public static Error KeepAliveFailed => new Error(nameof(KeepAliveFailed), "Failed to execute Keep Alive");
        public static Error DiscoveryNotSuccessful => new Error(nameof(DiscoveryNotSuccessful), "Couldn't accomplish the socket server discovery process");
    }
}
