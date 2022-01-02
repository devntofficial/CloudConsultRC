using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class GetKycMetadataProcessor : IQueryProcessor<GetKycMetadata, KycMetadataResponse>
    {
        private readonly IFileService fileService;
        private readonly IHostEnvironment env;
        private readonly IApiResponseBuilder<KycMetadataResponse> builder;

        public GetKycMetadataProcessor(IFileService fileService, IHostEnvironment env, IApiResponseBuilder<KycMetadataResponse> builder)
        {
            this.fileService = fileService;
            this.env = env;
            this.builder = builder;
        }

        public async Task<IApiResponse<KycMetadataResponse>> Handle(GetKycMetadata request, CancellationToken cancellationToken)
        {
            var kycDocumentPath = Path.Combine(env.ContentRootPath, $"kyc/{request.ProfileId}");
            if(!Directory.Exists(kycDocumentPath))
            {
                var response = new KycMetadataResponse { ProfileId = request.ProfileId, KycDocumentsMetadata = new() };
                return builder.CreateErrorResponse(response, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("No KYC documents found for given profile id");
                });
            }


            var kycDocuments = fileService.GetFilesInfoFromDirectory(kycDocumentPath, "*.*");
            if(kycDocuments.Count == 0)
            {
                var response = new KycMetadataResponse { ProfileId = request.ProfileId, KycDocumentsMetadata = new() };
                return builder.CreateErrorResponse(response, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("No KYC documents found for given profile id");
                });
            }

            var kycFileDetails = kycDocuments.Select(x => new KycMetadata
            {
                FileName = x.Name,
                FileSize = $"{x.Length / 1024} KB",
                UploadTimestamp = x.LastWriteTime
            }).ToList();

            return builder.CreateSuccessResponse(new KycMetadataResponse { ProfileId = request.ProfileId, KycDocumentsMetadata = kycFileDetails}, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("KYC file details fetched successfully");
            });
        }
    }
}
