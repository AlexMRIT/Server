using System;
using Server.Models;
using Server.Network;
using Server.Service;
using Server.Template;
using Server.Contracts;
using System.Threading.Tasks;
using Server.Network.InnerNetwork;
using Microsoft.Extensions.DependencyInjection;

namespace Server.RequestPacketHandler
{
    public sealed class RequestEnterRoom : NetworkPacketBaseImplement
    {
        private readonly Config ServerConfig;
        private readonly ClientProcessor Client;
        private readonly ICharacterServices CharacterServices;
        private readonly IServiceProvider ServiceProvider;

        private readonly int RoomId;
        private readonly ClientSession CurrentClientSession;

        public RequestEnterRoom(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Client = client;
            ServiceProvider = serviceProvider;
            ServerConfig = serviceProvider.GetService<Config>();
            CharacterServices = serviceProvider.GetService<ICharacterServices>();

            RoomId = packet.ReadInt();
            CurrentClientSession = new ClientSession(packet.InternalReadBool(), packet.InternalReadBool(), packet.InternalReadBool());
        }

        public override async Task ExecuteImplement()
        {
            if (CurrentClientSession != Client.CurrentSession)
            {
                Console.WriteLine($"Invalid SessionKey. AccountId: {Client.CurrectAccountContract.Id}");
                Client.Disconnect();
                return;
            }

            CharacterContract contract = await CharacterServices.GetCharacterByLoginAsync(Client.CurrectAccountContract.Login);
            CharacterTemplate template = new CharacterTemplate(new BaseSpecificationForCharacter());

            if (contract is null)
            {
                Console.WriteLine("Warning: Unregistered user found. Recreation is disabled. The client is disabled.");

                if (ServerConfig.AutoCreateCharacter)
                {
                    int id = IdFactory.Instance.NextId();
                    CharacterContract characterContract = new CharacterContract(id)
                    {
                        Score = 0,
                        Name = id.ToString(),
                        LoginName = Client.CurrectAccountContract.Login
                    };
                    contract = await CharacterServices.CreateCharacterAsync(characterContract);

                    if (contract is null)
                    {
                        Console.WriteLine("Critical service error. Character creation is not possible.");
                        Client.Disconnect();
                        return;
                    }
                }
            }

            CharacterEntity player = new CharacterEntity(ServiceProvider, template, Client);
            player.SetOnline(RoomId);

            Client.CurrentSession.SessionClientGamePlaying = true;
            await Client.WriteAsync(EnterRoom.ToPacket(player, Client.CurrentSession));
        }
    }
}