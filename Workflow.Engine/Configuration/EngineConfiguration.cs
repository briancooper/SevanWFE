namespace Workflow.Engine.Configuration
{
    public class EngineConfiguration : IEngineConfiguration
    {
        public string ConnectionString { get; }

        public EngineConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
