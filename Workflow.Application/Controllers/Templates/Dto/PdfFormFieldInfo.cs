using System.Collections.Generic;
using Workflow.Abstractions.Models;

namespace Workflow.Application.Controllers.Templates.Dto
{
    public class PdfFormFieldInfo : IPdfFormFieldInfo
    {
        public string Name { get; set; }
        public string Note { get; set; }        
        private List<string> _possibleValues;

        public List<string> PossibleValues
        {
            get => _possibleValues;
            set
            {
                _possibleValues = value;
                SetNote();
            }
        }

        public PdfFormFieldInfo()
        {
            
        }

        public PdfFormFieldInfo(string name, List<string> possibleValues)
        {
            Name = name;
            PossibleValues = possibleValues ?? new List<string>();
            SetNote();
        }

        private void SetNote()
        {
            if (PossibleValues.Count == 0)
            {
                Note = "Any value";
            }
            else
            {
                Note = $"Possible values: {string.Join(", ", PossibleValues.ToArray())}";
            }
        }
    }
}
