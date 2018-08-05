using System;

namespace Friday.Base.ValueTypes.Token
{
	[Serializable]
	public struct AuthToken : IEquatable<AuthToken>
	{
		public readonly Guid Id;


		public override string ToString()
		{
			return Id.ToString();
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

		public static AuthToken Create(string guid)
		{
			return new AuthToken(Guid.Parse(guid));
		}


		public static AuthToken Create(Guid id)
		{
			return new AuthToken(id);
		}

		public bool Equals(AuthToken other)
		{
			return Id.Equals(other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is AuthToken token && Equals(token);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}