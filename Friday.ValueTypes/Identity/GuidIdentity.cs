using System;

namespace Friday.ValueTypes.Identity
{
    public abstract class GuidIdentity : IEquatable<GuidIdentity>
    {
        public Guid Id { get; set; }

        protected GuidIdentity()
        {
            Id = Guid.NewGuid();
        }

        protected GuidIdentity(string id)
        {
            Id = Guid.Parse(id);
        }


        protected GuidIdentity(Guid id)
        {
            Id = id;
        }


        public static implicit operator Guid(GuidIdentity d)
        {
            return d.Id;
        }


        public bool Equals(GuidIdentity id)
        {
            if (ReferenceEquals(this, id)) return true;
            if (ReferenceEquals(null, id)) return false;
            return Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as GuidIdentity);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}