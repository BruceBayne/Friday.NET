using System;

namespace Friday.Base.Exceptions
{
    public class TypeNotFoundException : Exception
    {

        public TypeNotFoundException(string s) : base(s)
        {

        }
        public TypeNotFoundException(Type t) : base(t.ToString())
        {

        }
    }
}