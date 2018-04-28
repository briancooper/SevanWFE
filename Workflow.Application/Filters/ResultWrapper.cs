namespace Workflow.Application.Filters
{
    public class ResultWrapper
    {
        public object Result { get; set; }

        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public bool HasErrors { get; set; }

        public string ErrorMessage { get; set; }
    }
}
