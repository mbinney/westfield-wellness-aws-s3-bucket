westfield-wellness-finance / src / Finance.Services / Document /

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Finance.DataAccess.Repositories;
using Finance.Dto;
using Microsoft.Extensions.Logging;
using Finance.EntityFramework.Models;
using Finance.Helpers;

namespace Finance.Services
{
    public class InvoiceDocumentService : IInvoiceDocumentService
    {
        private readonly ILogger _logger;
        private readonly IInvoicePdfGenerator _invoicePdfGenerator;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFileStoreService _fileStoreService;
        private readonly IInvoiceDocumentRepository _invoiceDocumentRepository;
        private readonly IAmazonS3 _s3Client;
        private readonly ISettingsHelper _settingsHelper;
        private readonly IDocumentAccessTokenService _documentAccessTokenService;

        public InvoiceDocumentService(
            ILogger<InvoiceDocumentService> logger,
            IInvoicePdfGenerator pdfGenerator,
            IInvoiceRepository invoiceRepository,
            IFileStoreService fileStoreService,
            IInvoiceDocumentRepository invoiceDocumentRepository,
            IAmazonS3 s3Client,
            ISettingsHelper settingsHelper,
            IDocumentAccessTokenService documentAccessTokenService)
        {
            _logger = logger;
            _invoicePdfGenerator = pdfGenerator;
            _invoiceRepository = invoiceRepository;
            _fileStoreService = fileStoreService;
            _invoiceDocumentRepository = invoiceDocumentRepository;
            _s3Client = s3Client;
            _settingsHelper = settingsHelper;
            _documentAccessTokenService = documentAccessTokenService;
        }

        public async Task GenerateInvoiceDocument(InvoiceDocumentGenerationRequest req)
        {
            // Retrieve invoice data
            InvoiceDetailDto invoiceDetail = await _invoiceRepository.GetDetail(req.InvoiceId);

            var documentModel = new InvoiceDocumentGeneratorModel() { Invoice = invoiceDetail };

            // Render document
            var documentStream = _invoicePdfGenerator.GeneratePdf(documentModel);

            // Upload PDF to S3
            string bucketKey = $"finance/invoice/document-{Guid.NewGuid()}.pdf";

            var s3Result = await _fileStoreService.PutFile(documentStream, bucketKey);

            _logger.LogInformation("Generated invoice document for Invoice {invoiceId} with S3 Resource key {key} in bucket {bucket}", req.InvoiceId, s3Result.BucketKey, s3Result.BucketName);

            // Save document against Invoice
            var document = new InvoiceDocumentDto()
            {
                Type = "PDF",
                InvoiceId = invoiceDetail.Id,
                Document = new DocumentDto()
                {
                    BucketName = s3Result.BucketName,
                    ResourceKey = s3Result.BucketKey
                },
            };

            var result = await _invoiceDocumentRepository.AddAsync(document);

        }

        public async Task<string> GetTimeSensitiveURL(int invoiceDocumentId, int timePeriodInMins)
        {
            //https://docs.aws.amazon.com/AmazonS3/latest/dev/ShareObjectPreSignedURLDotNetSDK.html

            var invoiceDoc = await _invoiceDocumentRepository.GetInvoiceDocumentAsync(invoiceDocumentId);

            if (invoiceDoc is null)
            {
                return "Document does not exist so no URL created";
            }

            int.TryParse(_settingsHelper.GetMaxURLExpireTimeInMins(), out int result);

            if (timePeriodInMins > result)
            {
                return "Time period can't exceed " + result + " mins!";
            }

            //todo alter time element as config.
            var request1 = new GetPreSignedUrlRequest
            {
                BucketName = invoiceDoc.Document.BucketName,
                Key = invoiceDoc.Document.ResourceKey,
                Expires = DateTime.Now.AddMinutes(timePeriodInMins),
            };

            return _s3Client.GetPreSignedURL(request1);
        }

    }
}