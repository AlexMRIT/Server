using Server.Enums;
using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class SendDamageResult
    {
        internal static NetworkPacket ToPacket(DamageResult damageResult)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerDamageResult);

            packet.WriteByte((byte)damageResult);

            return packet;
        }
    }
}