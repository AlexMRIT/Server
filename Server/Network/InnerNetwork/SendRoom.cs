using Server.World;

namespace Server.Network.InnerNetwork
{
    public sealed class SendRoom
    {
        private const byte Opcode = 0x02;

        internal static NetworkPacket ToPacket(Room room)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            packet.WriteString(room.RoomName);
            packet.WriteString(room.RoomDescription);
            packet.WriteInt(room.CurrentCount);

            return packet;
        }
    }
}