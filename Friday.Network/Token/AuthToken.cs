using System;

namespace Friday.Network.Token
{
    [Serializable]
    public struct AuthToken
    {
        public readonly Guid Id;
        public readonly string UserAgent;
        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(UserAgent)}: {UserAgent}";
        }

        public AuthToken(Guid id, string userAgent)
        {
            Id = id;
            UserAgent = userAgent;
        }


        public AuthToken Create(string userAgent = "")
        {
            return new AuthToken(Guid.NewGuid(), userAgent);
        }


        public AuthToken Create(Guid id, string userAgent = "")
        {
            return new AuthToken(id, userAgent);
        }
    }
}