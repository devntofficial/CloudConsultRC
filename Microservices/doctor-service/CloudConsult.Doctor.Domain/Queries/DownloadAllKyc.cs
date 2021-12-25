using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Doctor.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Doctor.Domain.Queries;

public class DownloadAllKycDocuments : IQuery<KycDocumentResponse>
{
    public string ProfileId { get; set; }
}

public class DownloadKycDocumentsValidator : ApiValidator<DownloadAllKycDocuments>
{
    public DownloadKycDocumentsValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.ProfileId).Must(BeValidMongoDbId).WithMessage("'ProfileId' has invalid format");
    }
}
