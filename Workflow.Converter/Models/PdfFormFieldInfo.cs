using System.Collections.Generic;
using Workflow.Abstractions.Models;

namespace Workflow.Converter.Models
{
    public class PdfFormFieldInfo : IPdfFormFieldInfo
    {
        public string Name { get; set; }
        public List<string> PossibleValues { get; set; }

        public PdfFormFieldInfo(string name, List<string> possibleValues)
        {
            Name = name;
            PossibleValues = possibleValues ?? new List<string>();
        }
    }
}
