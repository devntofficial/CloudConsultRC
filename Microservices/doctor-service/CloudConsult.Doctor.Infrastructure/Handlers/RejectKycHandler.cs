using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class RejectKycHandler : ICommandHandler<RejectKyc>
    {
        private readonly IApiResponseBuilder builder;
        private readonly IKycService kycService;

        public RejectKycHandler(IApiResponseBuilder builder, IKycService kycService)
        {
            this.builder = builder;
            this.kycService = kycService;
        }

        public async Task<IApiResponse> Handle(RejectKyc request, CancellationToken cancellationToken)
        {
            var isApproved = await kycService.Reject(request, cancellationToken);

            if(isApproved)
            {
                return builder.CreateSuccessResponse(x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages("Doctor profile rejected successfully");
                });
            }

            return builder.CreateErrorResponse(x =>
            {
                x.WithErrorCode(StatusCodes.Status404NotFound);
                x.WithErrors("Doctor profile not found with given id");
            });
        }
    }
}
