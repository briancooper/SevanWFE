﻿using Microsoft.AspNetCore.Hosting;
using Shark.PdfConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Forms;
using iText.Kernel.Pdf;
using Workflow.Abstractions.Models;
using Workflow.Abstractions.Services;
using Workflow.Converter.Models;

namespace Workflow.Converter.Services
{
    public class ConverterService : IConverterService
    {
        private readonly IHostingEnvironment _hostingEnvironment;


        public ConverterService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> ConvertToPdfAsync(string content)
        {
            return await Task.Run(() =>
            {
                var headerUrl = $@"{_hostingEnvironment.ContentRootPath}\Reports\Default\header.html";

                var footerUrl = $@"{_hostingEnvironment.ContentRootPath}\Reports\Default\footer.html";
                var config = new PdfConversionSettings
                {
                    Title = "Workflow auto-generated report",
                    Content = content,
                    PageHeaderUrl = headerUrl,
                    PageFooterUrl = footerUrl,
                    Margins = new PdfPageMargins { Left = 10, Top = 17 + 20, Right = 10, Bottom = 20 },
                    OutputPath = $@"{_hostingEnvironment.ContentRootPath}\Reports\AutoGeneratedReport\{Guid.NewGuid()}.pdf",
                    CustomWkHtmlHeaderArgs = "--header-spacing 10",
                    CustomWkHtmlFooterArgs = "--footer-spacing 10",
                    PdfToolPath = $@"{_hostingEnvironment.ContentRootPath}\Utilities\wkhtmltopdf.exe"
                };

                PdfConvert.Convert(config);

                return config.OutputPath;
            });
        }

        public string FillPdfForm<T>(string filePath, List<T> fields) where T : IBindField
        {
            var outputFilePath =
                $"{_hostingEnvironment.ContentRootPath}\\FileTemplates\\{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss_ffff}.pdf";
            using (var reader = new PdfReader(filePath))
            {
                using (var fs = new FileStream(outputFilePath, FileMode.Create))
                {
                    using (var writer = new PdfWriter(fs))
                    {

                        using (var pdfDoc = new PdfDocument(reader, writer))
                        {
                            var form = PdfAcroForm.GetAcroForm(pdfDoc, false);
                            if (form == null)
                            {
                                return outputFilePath;
                            }

                            var pdfFields = form.GetFormFields();
                            foreach (var pdfField in pdfFields)
                            {
                                var field = fields.SingleOrDefault(x => x.From == pdfField.Key);
                                if (field == null)
                                    continue;

                                pdfField.Value.SetValue(field.To);
                            }
                        }
                    }
                }
            }

            return outputFilePath;
        }

        public IEnumerable<IPdfFormFieldInfo> GetPdfFormFieldsList(string filePath)
        {
            var fieldInfos = new List<PdfFormFieldInfo>();
            using (var reader = new PdfReader(filePath))
            {
                using (var pdfDocument = new PdfDocument(reader))
                {
                    var form = PdfAcroForm.GetAcroForm(pdfDocument, false);
                    if (form == null)
                    {
                        return fieldInfos;
                    }

                    var fields = form.GetFormFields();
                    foreach (var field in fields)
                    {
                        var possibleValues = field.Value.GetAppearanceStates().ToList();
                        fieldInfos.Add(new PdfFormFieldInfo(field.Key, possibleValues));
                    }
                }
            }

            return fieldInfos;
        }
    }
}
