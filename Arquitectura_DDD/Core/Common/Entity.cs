using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Arquitectura_DDD.Core.Common
{
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; protected set; }
        
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity other || other.GetType() != GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == default || other.Id == default)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
