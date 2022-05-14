using Server.World;
using Server.Utilite;
using System.Collections.Generic;

namespace Server.Network.InnerNetwork
{
    public sealed class SendRoom
    {
        internal static NetworkPacket ToPacket(List<Room> roomCollection, ClientSession session)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerSendRoom);

            foreach (Room room in roomCollection)
            {
                packet.WriteInt(room.RoomId);
                packet.WriteString(room.RoomName);
                packet.WriteString(room.RoomDescription);
                packet.WriteInt(room.GetCountObjects());
            }

            packet.InternalWriteBool(session.SessionClientAuthorization);
            packet.InternalWriteBool(session.SessionClientMatchSearch);
            packet.InternalWriteBool(session.SessionClientGamePlaying);

            return packet;
        }
    }
}