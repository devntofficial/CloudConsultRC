using CloudConsult.Common.Builders;
using CloudConsult.Common.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.Payment.Infrastructure.Clients
{
    public class ConsultationApiClient : CommonApiClient
    {
        public ConsultationApiClient(HttpClient client) : base(client)
        {

        }

        public async Task<IApiResponse<ConsultationResponse>> GetConsultationAsync(string consultationId, CancellationToken token = default)
        {
            var url = $"/api/v1/consultations/{consultationId}";
            return await GetAsync<ConsultationResponse>(url, token);
        }
    }
}
