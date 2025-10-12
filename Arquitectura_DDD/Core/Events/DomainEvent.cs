namespace Arquitectura_DDD.Core.Events
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }

        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }
}
