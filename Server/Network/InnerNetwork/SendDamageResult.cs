using Server.Enums;

namespace Server.Network.InnerNetwork
{
    public sealed class SendDamageResult
    {
        private const byte Opcode = 0x03;

        internal static NetworkPacket ToPacket(DamageResult damageResult)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            packet.WriteByte((byte)damageResult);

            return packet;
        }
    }
}