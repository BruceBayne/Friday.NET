using System;

namespace Friday.Base.ValueTypes.Token
{
    [Serializable]
    public struct AuthToken
    {
        public readonly Guid Id;


        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }

        public AuthToken(Guid id)
        {
            Id = id;
        }


        public static AuthToken Empty => new AuthToken(Guid.Empty);


        public static AuthToken Create()
        {
            return new AuthToken(Guid.NewGuid());
        }


        public static AuthToken Create(Guid id)
        {
            return new AuthToken(id);
        }
    }
}