using Server.Models;
using Server.Utilite;
using System.Collections.Generic;

namespace Server.Network.InnerNetwork
{
    public sealed class StatusUpdate
    {
        public static byte MaxHealth = 0x01;
        public static byte CurrentHealth = 0x02;

        public readonly List<KeyValuePair<byte, int>> Attributes = new List<KeyValuePair<byte, int>>(capacity: 2);

        public void AddAttribute(byte typeAttribute, int valueAttribute)
        {
            Attributes.Add(new KeyValuePair<byte, int>(typeAttribute, valueAttribute));
        }

        internal static NetworkPacket ToPacket(StatusUpdate update, CharacterEntity character)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerCharacterUpdate);

            packet.WriteInt(character.Template.Id);
            packet.WriteInt(update.Attributes.Count);
            foreach (KeyValuePair<byte, int> pair in update.Attributes)
            {
                packet.WriteByte(pair.Key);
                packet.WriteInt(pair.Value);
            }

            return packet;
        }
    }
}