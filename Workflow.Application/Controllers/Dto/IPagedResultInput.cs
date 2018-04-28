namespace Workflow.Application.Controllers.Dto
{
    public interface IPagedResultInput
    {
        int MaxResultCount { get; }

        int SkipCount { get; }
    }
}
