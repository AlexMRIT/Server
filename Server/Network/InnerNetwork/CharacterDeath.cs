namespace Server.Network.InnerNetwork
{
    public sealed class CharacterDeath
    {
        private static readonly byte Opcode = 0x07;

        internal static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            return packet;
        }
    }
}