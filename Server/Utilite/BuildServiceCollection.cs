using Server.World;
using Server.Service;
using Server.Network;
using Server.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Utilite
{
    public static class BuildServiceCollection
    {
        public static void Build(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<Config>();
            serviceDescriptors.AddSingleton<GameServer>();
            serviceDescriptors.AddSingleton<ThreadsRoom>();
            serviceDescriptors.AddSingleton<ServerHandler>();
            serviceDescriptors.AddSingleton<ClientHandler>();
            serviceDescriptors.AddSingleton<PreCheckStartServiceRepository>();

            serviceDescriptors.AddSingleton<ICheckService, CheckRepository>();
            serviceDescriptors.AddSingleton<IAccountServices, AccountServices>();
            serviceDescriptors.AddSingleton<IGetAllIdsInitialise, GetAllIdsInitialise>();
        }
    }
}