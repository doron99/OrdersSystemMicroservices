using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DomainEvents
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event);
    }
}
