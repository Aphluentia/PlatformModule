namespace Backend.Models.Entities
{
    public class User
    {
		public Guid? WebPlatformId { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public ISet<string>? Modules { get; set; }
		public ISet<string>? ActiveScenarioIds { get; set; }
		public string Password { get; set; }
		public int? PermissionLevel { get; set; }
    }
}

		