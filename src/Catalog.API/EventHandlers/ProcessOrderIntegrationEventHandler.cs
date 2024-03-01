using Catalog.API.Data;
using Common;
using EventBus;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.EventHandlers
{
    public class ProcessOrderIntegrationEventHandler : IIntegrationEventHandler<ProcessOrderIntegrationEvent>
    {
        private readonly ApplicationDbContext _context;
        public ProcessOrderIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ProcessOrderIntegrationEvent @event)
        {
            foreach(var productItem in @event.Products)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productItem.ProductId);
                if (product != null)
                {
                    product.UnitInStock -= productItem.Quantity;
                    _context.Update(product);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
