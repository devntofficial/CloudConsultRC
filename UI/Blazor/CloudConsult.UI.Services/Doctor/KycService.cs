using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Services.Routes;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services.Doctor
{
    public class KycService : IKycService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;

        public KycService(HttpClient client)
        {
            this.client = client;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse> Upload(string profileId, List<IBrowserFile> kycDocuments)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                kycDocuments.ForEach(kycDocument =>
                {
                    var kycDocumentContent = new StreamContent(kycDocument.OpenReadStream(kycDocument.Size));
                    kycDocumentContent.Headers.ContentType = new MediaTypeHeaderValue(kycDocument.ContentType);
                    content.Add(
                        content: kycDocumentContent,
                        name: "\"KycDocuments\"",
                        fileName: kycDocument.Name);
                });

                var response = await client.PostAsync(GatewayRoutes.DoctorService.KycUpload.Replace("{ProfileId}", profileId), content);
                return await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }
    }
}
