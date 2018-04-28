namespace Workflow.Engine.Services.Action.Dto
{
    public class ContentDtoInput
    {
        public string Source { get; set; }

        public string Result { get; set; }


        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Source) || string.IsNullOrWhiteSpace(Result))
            {
                return false;
            }

            return true;
        }
    }
}
