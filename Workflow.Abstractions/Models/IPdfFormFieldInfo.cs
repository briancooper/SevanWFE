using System.Collections.Generic;

namespace Workflow.Abstractions.Models
{
    public interface IPdfFormFieldInfo
    {
        string Name { get; set; }
        List<string> PossibleValues { get; set; }
    }
}
