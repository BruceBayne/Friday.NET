using System;

namespace Friday.Base.Exceptions
{
	public class TypeNotFoundException : Exception
	{

		public TypeNotFoundException(string message) : base(message)
		{

		}
		public TypeNotFoundException(Type t) : base(t.ToString())
		{

		}
	}
}