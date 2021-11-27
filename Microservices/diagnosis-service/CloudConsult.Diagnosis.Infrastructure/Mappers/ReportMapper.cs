using AutoMapper;
using CloudConsult.Diagnosis.Domain.Commands;
using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Responses;

namespace CloudConsult.Diagnosis.Infrastructure.Mappers
{
    public class ReportMapper : Profile
    {
        public ReportMapper()
        {
            CreateMap<UploadReport, DiagnosisReport>();
            CreateMap<DiagnosisReport, UploadReportResponse>()
                .ForMember(x => x.ReportId, y => y.MapFrom(z => z.Id.ToString()));
        }
    }
}
