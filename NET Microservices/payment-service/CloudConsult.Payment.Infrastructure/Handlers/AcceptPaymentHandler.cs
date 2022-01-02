using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Common.Enums;
using CloudConsult.Payment.Domain.Commands;
using CloudConsult.Payment.Domain.Events;
using CloudConsult.Payment.Domain.Responses;
using CloudConsult.Payment.Infrastructure.Clients;
using Kafka.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CloudConsult.Payment.Infrastructure.Handlers
{
    public class AcceptPaymentHandler : ICommandHandler<AcceptPayment, PaymentResponse?>
    {
        private readonly IApiResponseBuilder<PaymentResponse?> builder;
        private readonly ClusterClient kafkaCluster;
        private readonly IConfiguration config;
        private readonly ConsultationApiClient consultationApiClient;

        public AcceptPaymentHandler(IApiResponseBuilder<PaymentResponse?> builder, ClusterClient kafkaCluster, IConfiguration config,
            ConsultationApiClient consultationApiClient)
        {
            this.builder = builder;
            this.kafkaCluster = kafkaCluster;
            this.config = config;
            this.consultationApiClient = consultationApiClient;
        }
        public async Task<IApiResponse<PaymentResponse?>> Handle(AcceptPayment request, CancellationToken cancellationToken)
        {
            var consultationApiResponse = await consultationApiClient.GetConsultationAsync(request.ConsultationId, cancellationToken);
            if (consultationApiResponse.IsSuccess == false)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors(consultationApiResponse.Errors);
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                });
            }

            var consultation = consultationApiResponse.Payload;
            if(consultation.Status != ConsultationEvents.ConsultationAccepted.ToString())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors("Cannot accept payment for this consultation");
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                });
            }


            var response = new PaymentResponse
            {
                ConsultationId = request.ConsultationId,
                Status = "Accepted"
            };

            var paymentEvent = new PaymentAccepted
            {
                ConsultationId = response.ConsultationId,
                PaymentId = response.PaymentId,
                Status = response.Status
            };

            kafkaCluster.Produce(config["KafkaConfiguration:PaymentAcceptedTopic"], paymentEvent);
            return await Task.FromResult(builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Payment accepted successfully");
            }));
        }
    }
}
