using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class CharacterMoveToLocation
    {
        private const byte Opcode = 0x05;

        internal static NetworkPacket ToPacket(int id, Vector3 direction)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            packet.WriteInt(id);

            packet.WriteDouble(direction.x);
            packet.WriteDouble(direction.y);
            packet.WriteDouble(direction.z);

            return packet;
        }
    }
}