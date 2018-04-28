namespace Workflow.Security.Configuration
{
    public class SecurityConfiguration : ISecurityConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }
    }
}
