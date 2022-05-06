namespace Server.Network.InnerNetwork
{
    public static class ServerLoginFail
    {
        private const byte Opcode = 0x00;

        internal static NetworkPacket ToPacket(byte opCode)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);
            packet.WriteByte(opCode);

            return packet;
        }
    }
}