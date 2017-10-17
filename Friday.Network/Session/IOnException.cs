using System;

namespace Friday.Network.Session
{
    public interface IOnException
    {
        void OnException(Exception e);
    }
}