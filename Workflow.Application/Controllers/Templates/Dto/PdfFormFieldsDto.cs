using System;
using System.Collections.Generic;
using Workflow.Abstractions.Models;

namespace Workflow.Application.Controllers.Templates.Dto
{
    public class PdfFormFieldsDto
    {
        public Guid Id { get; set; }

        public List<PdfFormFieldInfo> FieldInfoes { get; set; }

        public PdfFormFieldsDto(Guid id, List<PdfFormFieldInfo> fieldInfos)
        {
            Id = id;
            FieldInfoes = fieldInfos;
        }
    }
}
