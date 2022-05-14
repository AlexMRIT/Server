using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public static class ServerLoginFail
    {
        internal static NetworkPacket ToPacket(byte opCode)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerLoginFail);
            packet.WriteByte(opCode);

            return packet;
        }
    }
}