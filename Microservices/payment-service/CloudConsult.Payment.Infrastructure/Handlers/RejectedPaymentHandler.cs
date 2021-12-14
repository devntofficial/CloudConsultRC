using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Payment.Domain.Commands;
using CloudConsult.Payment.Domain.Events;
using CloudConsult.Payment.Domain.Responses;
using Kafka.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CloudConsult.Payment.Infrastructure.Handlers
{
    public class RejectPaymentHandler : ICommandHandler<RejectPayment, PaymentResponse>
    {
        private readonly IApiResponseBuilder<PaymentResponse> builder;
        private readonly IClusterClient kafkaCluster;
        private readonly IConfiguration config;

        public RejectPaymentHandler(IApiResponseBuilder<PaymentResponse> builder, ClusterClient kafkaCluster, IConfiguration config)
        {
            this.builder = builder;
            this.kafkaCluster = kafkaCluster;
            this.config = config;
        }
        public Task<IApiResponse<PaymentResponse>> Handle(RejectPayment request, CancellationToken cancellationToken)
        {
            var response = new PaymentResponse
            {
                ConsultationId = request.ConsultationId,
                Status = "Rejected"
            };

            var paymentEvent = new PaymentRejected
            {
                ConsultationId = response.ConsultationId,
                PaymentId = response.PaymentId,
                Status = response.Status
            };

            kafkaCluster.Produce(config["KafkaConfiguration:PaymentRejectedTopic"], paymentEvent);
            return Task.FromResult(builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Payment rejected successfully");
            }));
        }
    }
}
