using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Domain.Commands
{
    public class UploadKycDocuments : ICommand
    {
        public string ProfileId { get; set; }
        public List<IFormFile> KycDocuments { get; set; }
    }

    public class UploadKycDocumentsValidator : ApiValidator<UploadKycDocuments>
    {
        public UploadKycDocumentsValidator()
        {
            RuleFor(x => x.ProfileId).NotEmpty();
            RuleFor(x => x.ProfileId).Must(BeValidMongoDbId).WithMessage("'ProfileId' has invalid format");
            RuleFor(x => x.KycDocuments).NotEmpty();
        }
    }
}
