namespace Workflow.Security.Configuration
{
    public interface ISecurityConfiguration
    {
        string Issuer { get; set; }

        string Audience { get; set; }

        string SecretKey { get; set; }
    }
}
