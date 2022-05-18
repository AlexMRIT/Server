using System;
using Server.Enums;
using Server.RequestPacketHandler;
using System.Collections.Concurrent;

namespace Server.Network
{
    public sealed class GamePacketHandler
    {
        private readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();
        private readonly IServiceProvider ServiceProvider;

        public GamePacketHandler(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            ClientPackets.TryAdd((byte)Opcode.RequestAuthorization, typeof(RequestAuthorization));
            ClientPackets.TryAdd((byte)Opcode.RequestGetRoomsWindow, typeof(RequestGetRoomsWindow));
            ClientPackets.TryAdd((byte)Opcode.RequestMovementAsync, typeof(RequestMovementAsync));
            ClientPackets.TryAdd((byte)Opcode.RequestAttackHandle, typeof(RequestAttackHandle));
            ClientPackets.TryAdd((byte)Opcode.RequestEnterRoom, typeof(RequestEnterRoom));
            ClientPackets.TryAdd((byte)Opcode.RequestMovementStopSync, typeof(RequestMovementStopSync));
            ClientPackets.TryAdd((byte)Opcode.RequestCreateServer, typeof(RequestCreateServer));
        }

        public void HandlePacket(NetworkPacket packet, ClientProcessor client)
        {
            Console.WriteLine($"Received packet: {packet.FirstOpcode:X2}:{packet.SecondOpcode:X2}");
            Console.WriteLine(packet.ToString());

            NetworkPacketBaseImplement networkPacket = null;

            if (ClientPackets.ContainsKey(packet.FirstOpcode))
            {
                Console.WriteLine($"Received packet of type: {ClientPackets[packet.FirstOpcode].Name}");
                networkPacket = (NetworkPacketBaseImplement)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], ServiceProvider, packet, client);
            }

            if (networkPacket == null)
                throw new ArgumentNullException(nameof(NetworkPacketBaseImplement), $"Packet with opcode: {packet.FirstOpcode:X2} doesn't exist in the dictionary.");

            networkPacket.ExecuteImplement();
        }
    }
}