﻿using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class StopMove
    {
        private const byte Opcode = 0x04;

        internal static NetworkPacket ToPacket(int id, Vector3 position)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            packet.WriteInt(id);

            packet.WriteDouble(position.x);
            packet.WriteDouble(position.y);
            packet.WriteDouble(position.z);

            return packet;
        }
    }
}