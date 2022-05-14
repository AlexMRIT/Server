using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class StopMove
    {
        internal static NetworkPacket ToPacket(int id, Vector3 position)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerCharacterStopMove);

            packet.WriteInt(id);

            packet.WriteDouble(position.x);
            packet.WriteDouble(position.y);
            packet.WriteDouble(position.z);

            return packet;
        }
    }
}