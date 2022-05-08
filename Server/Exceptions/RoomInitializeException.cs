using System;

namespace Server.Exceptions
{
    public sealed class RoomInitializeException : Exception
    {
        public RoomInitializeException(string message)
            : base(message)
        { }

        public override string Message => base.Message;
    }
}