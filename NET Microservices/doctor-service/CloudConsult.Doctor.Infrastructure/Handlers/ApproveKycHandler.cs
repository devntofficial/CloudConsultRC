﻿using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class ApproveKycHandler : ICommandHandler<ApproveKyc>
    {
        private readonly IApiResponseBuilder builder;
        private readonly IKycService kycService;

        public ApproveKycHandler(IApiResponseBuilder builder, IKycService kycService)
        {
            this.builder = builder;
            this.kycService = kycService;
        }

        public async Task<IApiResponse> Handle(ApproveKyc request, CancellationToken cancellationToken)
        {
            var isApproved = await kycService.Approve(request, cancellationToken);

            if(isApproved)
            {
                return builder.CreateSuccessResponse(x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages("Doctor profile approved successfully");
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
