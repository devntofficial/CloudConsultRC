using CloudConsult.Common.CQRS;
using System.Text.Json.Serialization;

namespace CloudConsult.Doctor.Domain.Commands;

public class RejectKyc : ICommand
{
    [JsonIgnore]
    public string ProfileId { get; set; }
    [JsonIgnore]
    public string RejectionIdentityId { get; set; }
    public string RejectionComments { get; set; }
}
