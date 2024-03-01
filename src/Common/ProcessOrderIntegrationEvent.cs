using EventBus.SharedModels;

namespace Common
{
    public record ProcessOrderIntegrationEvent : IntegrationEvent
    {
        public List<ProductInOrderDto>? Products { get; set; }
    }

}
