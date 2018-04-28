namespace Workflow.Engine.Services.Action.Dto
{
    public class AttributeDtoInput
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Expression { get; set; }


        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Value))
            {
                return false;
            }

            return true;
        }
    }
}
