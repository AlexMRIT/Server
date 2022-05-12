namespace Server.Network.InnerNetwork
{
    public sealed class ExitAllRoomCmd
    {
        private static readonly byte Opcode = 0x08;

        internal static NetworkPacket ToPacket()
        {
            return new NetworkPacket(Opcode);
        }
    }
}