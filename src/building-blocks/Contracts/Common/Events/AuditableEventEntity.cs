using Contracts.Domains.Interfaces;

namespace Contracts.Common.Events;

public abstract class AuditableEventEntity<TKey> : EventEntity<TKey>, IAuditable, IEventEntity<TKey>
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}