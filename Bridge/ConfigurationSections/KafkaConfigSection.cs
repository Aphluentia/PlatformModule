namespace Bridge.ConfigurationSections
{
    public class KafkaConfigSection
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }

    }
}
