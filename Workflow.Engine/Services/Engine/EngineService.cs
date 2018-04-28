using OptimaJet.Workflow;
using OptimaJet.Workflow.Core.Builder;
using OptimaJet.Workflow.Core.Bus;
using OptimaJet.Workflow.Core.Parser;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.DbPersistence;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Workflow.Abstractions.Services;
using Workflow.Engine.Configuration;

namespace Workflow.Engine.Services
{
    public class EngineService : IEngineService
    {
        private readonly WorkflowRuntime _runtime;

        private readonly IActionService _actionService;

        private readonly IEngineConfiguration _engineConfiguration;


        public EngineService(IActionService actionService, IEngineConfiguration engineConfiguration)
        {
            _actionService = actionService;

            _engineConfiguration = engineConfiguration;

            _runtime = WorkflowRuntimeFactory();
        }

        public string Designer(NameValueCollection parameters, Stream stream)
        {
            return _runtime.DesignerAPI(parameters, stream);
        }

        private WorkflowRuntime WorkflowRuntimeFactory()
        {
            //WorkflowRuntime.RegisterLicense("your license key text");
            var dbProvider = new MSSQLProvider(_engineConfiguration.ConnectionString);

            var builder = new WorkflowBuilder<XElement>(dbProvider, new XmlWorkflowParser(), dbProvider).WithDefaultCache();

            var runtime = new WorkflowRuntime()
                .WithBuilder(builder)
                .WithPersistenceProvider(dbProvider)
                .WithActionProvider(_actionService)
                .WithBus(new NullBus())
                .EnableCodeActions()
                .SwitchAutoUpdateSchemeBeforeGetAvailableCommandsOn();

            runtime.ProcessStatusChanged += Runtime_ProcessStatusChanged;

            runtime.Start();

            return runtime;
        }

        private void Runtime_ProcessStatusChanged(object sender, ProcessStatusChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public async Task RunSchemeAsync(string code, Guid id)
        {
            if (!_runtime.IsProcessExists(id))
            {
                await _runtime.CreateInstanceAsync(code, id);
            }
        }
    }
}
