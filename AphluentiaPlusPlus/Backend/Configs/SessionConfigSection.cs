namespace Backend.Configs
{
    public class SessionConfigSection
    {
        public int SessionValidityInMinutes { get; set; }
        public int KeepAliveValidityInMinutes { get; set; }
    }
}
