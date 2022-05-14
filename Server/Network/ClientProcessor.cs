using System;
using System.Net;
using Server.Models;
using Server.Utilite;
using Server.Contracts;
using Server.Exceptions;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

#pragma warning disable CS4014

namespace Server.Network
{
    public sealed class ClientProcessor
    {
        public ClientHandler ManagerClient { get; }
        public TcpClient StreamClient { get; }
        public NetworkStream NetworkStreamClient { get; }
        public GamePacketHandler PacketHandler { get; }
        public EndPoint Address { get; }
        public bool IsDisconnected { get; private set; }
        public CharacterEntity CurrentCharacter { get; set; }
        public ClientSession CurrentSession { get; private set; }
        public AccountContract CurrectAccountContract { get; set; }

        private const int OpCodeLength = 2;

        public ClientProcessor(ClientHandler clientHandler, TcpClient tcpClient, GamePacketHandler gamePacketHandler)
        {
            Console.WriteLine($"Connection from {tcpClient.Client.RemoteEndPoint}");

            Address = tcpClient.Client.RemoteEndPoint;
            ManagerClient = clientHandler;
            StreamClient = tcpClient;
            NetworkStreamClient = tcpClient.GetStream();
            PacketHandler = gamePacketHandler;
            CurrentSession = new ClientSession(authorization: true, matchSearch: false, gamePlaying: false);

            Task.Factory.StartNew(ReadAsync);
        }

        public async Task WriteAsync(NetworkPacket packet)
        {
            if (IsDisconnected)
                return;

            byte[] buffer = packet.GetBuffer();
            List<byte> bytesBuild = new List<byte>();
            bytesBuild.BuildBufferWithOpcodePacket(buffer);

            try
            {
                await NetworkStreamClient.WriteAsync(bytesBuild.ToArray(), 0, bytesBuild.Count);
                await NetworkStreamClient.FlushAsync();
            }
            catch
            {
                Console.WriteLine($"Client {CurrectAccountContract.Id} terminated.");
                Disconnect();
            }
        }

        public async void ReadAsync()
        {
            try
            {
                while (true)
                {
                    if (IsDisconnected)
                        return;

                    byte[] _buffer = new byte[OpCodeLength];
                    int bytesRead = await NetworkStreamClient.ReadAsync(_buffer, 0, OpCodeLength);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Client closed connection");
                        Disconnect();
                        return;
                    }

                    if (bytesRead != OpCodeLength)
                        throw new NetworkPacketException("Wrong packet");

                    short length = BitConverter.ToInt16(_buffer, 0);
                    _buffer = new byte[length - OpCodeLength];

                    bytesRead = await NetworkStreamClient.ReadAsync(_buffer, 0, length - OpCodeLength);

                    if (bytesRead != length - OpCodeLength)
                        throw new NetworkPacketException("Wrong packet");

                    Task.Factory.StartNew(() => PacketHandler.HandlePacket(_buffer.ToPacket(), this));
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(ClientProcessor.ReadAsync));
                Disconnect();
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Call termination client.");

            try
            {
                if ((CurrentCharacter?.Online).Value)
                    CurrentCharacter.SetOffline();
            }
            catch (NullReferenceException exception)
            {
                ExceptionHandler.Execute(exception, nameof(ClientProcessor.Disconnect));
            }

            IsDisconnected = true;
            StreamClient.Close();
            NetworkStreamClient.Close();

            ManagerClient.ClientDisconnectFromRegister(Address.ToString());
        }

        public bool LoggedAlready(int id)
        {
            return (ManagerClient.LoggedAlready(Address.ToString()).CurrectAccountContract?.Id.Equals(id)).Value;
        }

        public void LoggedOtherAccountDisconnect()
        {
            ManagerClient.LoggedAlready(Address.ToString()).Disconnect();
        }
    }
}