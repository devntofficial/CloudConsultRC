using CloudConsult.Common.CQRS;
using System.Text.Json.Serialization;

namespace CloudConsult.Doctor.Domain.Commands;

public class ApproveKyc : ICommand
{
    [JsonIgnore]
    public string ProfileId { get; set; } = string.Empty;
    public string ApprovalIdentityId { get; set; } = string.Empty;
    public string ApprovalComments { get; set; } = string.Empty;
}
