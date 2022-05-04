namespace Server
{
    public sealed class Config
    {
        public readonly int MaxRoomGame = 10;
        public readonly int MaxSendPackets = 30;
        public readonly int TickSendPackets = 1000;
        public readonly int MaxCountPlayerForRoom = 2;
        public readonly int MaxOpenRoomPreStartServer = 1;
    }
}