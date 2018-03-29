using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Friday.Network.Transport
{

	[Serializable]
	public struct AuthToken
	{
		public readonly Guid Id;
		public readonly string UserAgent;
		public readonly DateTime CreationDate;
		public readonly IPAddress IpAddress;

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(UserAgent)}: {UserAgent}, {nameof(CreationDate)}: {CreationDate}, {nameof(IpAddress)}: {IpAddress}";
		}

		public AuthToken(Guid id, string userAgent, DateTime creationDate, IPAddress ipAddress)
		{
			Id = id;
			UserAgent = userAgent;
			CreationDate = creationDate;
			IpAddress = ipAddress;
		}


		public AuthToken Create(Guid id, string userAgent = "")
		{
			return new AuthToken(id, userAgent, DateTime.Now, IPAddress.Any);
		}


		public AuthToken Create(Guid id, IPAddress address, string userAgent = "")
		{
			return new AuthToken(id, userAgent, DateTime.Now, address);
		}



		public AuthToken Create(Guid id, IPAddress address, DateTime dateTime, string userAgent = "")
		{
			return new AuthToken(id, userAgent, dateTime, address);
		}


	}
}
