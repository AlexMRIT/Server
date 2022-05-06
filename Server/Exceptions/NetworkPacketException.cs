using System;

namespace Server.Exceptions
{
    public sealed class NetworkPacketException : Exception
    {
        public NetworkPacketException(string message)
            : base(message)
        { }

        public override string Message => base.Message;
    }
}