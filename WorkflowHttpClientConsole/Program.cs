using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowHttpClient.Clients;

namespace WorkflowHttpClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute().Wait();

        }

        static async Task Execute()
        {
            TriggerClient client = new TriggerClient("http://localhost:4300/", "SqPJjhTCpBHS0pi/jdIRsdEb5vMGDHe5/t+L+98Eamo=");
            //var response = await client.GetAllSchemas(100, 0);
            //var response1 = await client.GetSchema("test");
            var dic = new Dictionary<string, string> { { "1", "2" } };
            var param = JsonConvert.SerializeObject(new
            {
                UserEmailAddress = "myhaylo.stillwaters@gmail.com",
                UserTestResultInput = "60",
                UserName = "mike",
                TestName = "Entry Form",
                CertificateName = "Entry Sertificate",
                TrainingId = new Guid()
            }
            );
            var t = JsonConvert.SerializeObject(new
            {
                UserEmailAddress = "myhaylo.stillwaters@gmail.com",
                UserTestResultInput = 60,
                UserName = "mike",
                TestName = "Entry Form",
                CertificateName = "Entry Sertificate",
                TrainingId = new Guid()
            });
            var t1 = JObject.Parse(t);
            var response2 = await client.Fire("testTrigger", new
            {
                UserEmailAddress = "myhaylo.stillwaters@gmail.com",
                UserTestResultInput = 60,
                UserName = "mike",
                TestName = "Entry Form",
                CertificateName = "Entry Sertificate",
                TrainingId = 12213132
            });
            Console.WriteLine(response2);
            Console.ReadLine();
        }
    }
}
