using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CloudConsult.Doctor.Infrastructure.Handlers;

public class UploadKycDocumentsHandler : ICommandHandler<UploadKycDocuments>
{
    private readonly IApiResponseBuilder builder;
    private readonly IHostingEnvironment env;
    private readonly IValidator<UploadKycDocuments> validator;

    public UploadKycDocumentsHandler(IApiResponseBuilder builder, IHostingEnvironment env, IValidator<UploadKycDocuments> validator)
    {
        this.builder = builder;
        this.env = env;
        this.validator = validator;
    }

    public async Task<IApiResponse> Handle(UploadKycDocuments request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (validation.Errors.Any())
        {
            return builder.CreateErrorResponse(x =>
            {
                x.WithErrorCode(StatusCodes.Status400BadRequest);
                x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
            });
        }

        var kycDocumentPath = Path.Combine(env.ContentRootPath, $"kyc/{request.ProfileId}");
        if (!Directory.Exists(kycDocumentPath)) Directory.CreateDirectory(kycDocumentPath);

        request.KycDocuments.ForEach(file =>
        {
            if (file.Length <= 0) return;

            var filePath = Path.Combine(kycDocumentPath, file.FileName);
            if (File.Exists(filePath)) File.Delete(filePath);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
        });

        return builder.CreateSuccessResponse(x =>
        {
            x.WithSuccessCode(StatusCodes.Status200OK);
            x.WithMessages("Kyc documents were uploaded successfully");
        });
    }
}
