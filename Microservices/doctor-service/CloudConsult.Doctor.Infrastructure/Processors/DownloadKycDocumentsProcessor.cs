using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class DownloadKycDocumentsProcessor : IQueryProcessor<DownloadKycDocuments, KycDocumentResponse>
    {
        private readonly IApiResponseBuilder<KycDocumentResponse> builder;
        private readonly IHostingEnvironment env;
        private readonly IValidator<DownloadKycDocuments> validator;

        public DownloadKycDocumentsProcessor(IApiResponseBuilder<KycDocumentResponse> builder, IHostingEnvironment env, IValidator<DownloadKycDocuments> validator)
        {
            this.builder = builder;
            this.env = env;
            this.validator = validator;
        }

        public async Task<IApiResponse<KycDocumentResponse>> Handle(DownloadKycDocuments request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (validation.Errors.Any())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var kycDocumentPath = Path.Combine(env.ContentRootPath, $"kyc/{request.ProfileId}");

            if (!Directory.Exists(kycDocumentPath) || !Directory.EnumerateFiles(kycDocumentPath).Any())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Kyc documents not found for given profile id");
                });
            }

            var zipName = $"kyc-documents-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.zip";
            var kycDocuments = Directory.GetFiles(kycDocumentPath).ToList();

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                kycDocuments.ForEach(kycDocument =>
                {
                    var file = archive.CreateEntry(kycDocument);
                    using var streamWriter = new StreamWriter(file.Open());
                    streamWriter.Write(File.ReadAllText(kycDocument));
                });
            }

            var response = new KycDocumentResponse
            {
                FileType = "application/zip",
                ArchiveName = zipName,
                ArchiveData = memoryStream.ToArray()
            };

            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Kyc documents were downloaded successfully");
            });
        }
    }
}
