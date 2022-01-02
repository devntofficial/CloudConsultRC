using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class DownloadOneKycProcessor : IQueryProcessor<DownloadOneKyc, KycDocumentResponse>
    {
        private readonly IApiResponseBuilder<KycDocumentResponse> builder;
        private readonly IHostEnvironment env;

        public DownloadOneKycProcessor(IApiResponseBuilder<KycDocumentResponse> builder, IHostEnvironment env)
        {
            this.builder = builder;
            this.env = env;
        }

        public async Task<IApiResponse<KycDocumentResponse>> Handle(DownloadOneKyc request, CancellationToken cancellationToken)
        {
            var kycDocumentPath = Path.Combine(env.ContentRootPath, $"kyc/{request.ProfileId}");
            if (!Directory.Exists(kycDocumentPath) || !Directory.EnumerateFiles(kycDocumentPath).Any())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Kyc document not found");
                });
            }

            var kycDocumentFilePath = $"{kycDocumentPath}/{request.FileName}";
            if (!File.Exists(kycDocumentFilePath))
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Kyc document not found");
                });
            }

            var response = new KycDocumentResponse
            {
                FileName = request.FileName,
                FileType = "application/octet-stream",
                FileData = File.ReadAllBytes(kycDocumentFilePath)
            };
            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Kyc document found");
            });
        }
    }
}
