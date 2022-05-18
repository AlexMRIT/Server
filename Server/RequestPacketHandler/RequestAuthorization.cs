using System;
using Server.Network;
using Server.Service;
using Server.Contracts;
using System.Threading.Tasks;
using Server.Network.Messages;
using Server.Network.InnerNetwork;
using Microsoft.Extensions.DependencyInjection;

namespace Server.RequestPacketHandler
{
    public sealed class RequestAuthorization : NetworkPacketBaseImplement
    {
        private readonly IAccountServices AccountService;
        private readonly Config ServerConfig;
        private readonly ClientProcessor Client;

        private readonly ClientSession CurrentClientSession;
        private readonly string Login;
        private readonly string Password;

        public RequestAuthorization(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            AccountService = serviceProvider.GetService<IAccountServices>();
            ServerConfig = serviceProvider.GetService<Config>();
            Client = client;

            CurrentClientSession = new ClientSession(packet.InternalReadBool(), packet.InternalReadBool(), packet.InternalReadBool());

            Login = packet.ReadString(packet.ReadInt());
            Password = packet.ReadString(packet.ReadInt());
        }

        public override async Task ExecuteImplement()
        {
            if (Client.CurrectAccountContract != null)
            {
                Client.Disconnect();
                return;
            }

            if (CurrentClientSession != Client.CurrentSession)
            {
                Console.WriteLine($"Invalid SessionKey. AccountId: {Client.CurrectAccountContract.Id}");
                Client.Disconnect();
                return;
            }

            AccountContract accountContract = await AccountService.GetAccountByLoginAsync(Login);

            if (accountContract == null)
            {
                if (ServerConfig.AutoCreateAccount)
                    accountContract = await AccountService.CreateAccountAsync(Login, Password);
                else
                {
                    await Client.WriteAsync(ServerLoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    return;
                }
            }
            else
            {
                if (!accountContract.Password.Equals(Password))
                {
                    await Client.WriteAsync(ServerLoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    return;
                }

                if (Client.LoggedAlready())
                {
                    await Client.WriteAsync(ServerLoginFail.ToPacket(LoginFailReason.ReasonAccountInUse));
                    Client.LoggedOtherAccountDisconnect();
                    return;
                }
            }

            Client.CurrectAccountContract = accountContract;
            Client.CurrentSession.SessionClientAuthorization = true;
            await Client.WriteAsync(LoginOk.ToPacket(Client));
            Console.WriteLine($"Client: {Client.Address} has authentication!");
        }
    }
}