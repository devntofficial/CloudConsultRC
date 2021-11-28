using CloudConsult.Common.CQRS;
using System.Text.Json.Serialization;

namespace CloudConsult.Doctor.Domain.Commands;

public class ApproveProfile : ICommand
{
    [JsonIgnore]
    public string ProfileId { get; set; }
    public string ApprovalIdentityId { get; set; }
    public string ApprovalComments { get; set; }
}
