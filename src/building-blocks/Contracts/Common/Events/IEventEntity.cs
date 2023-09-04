using Contracts.Domains.Interfaces;

namespace Contracts.Common.Events;

public interface IEventEntity
{
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
    IReadOnlyCollection<BaseEvent> DomainEvents();
}

public interface IEventEntity<TKey> : IEntityBase<TKey>, IEventEntity
{
}