using System;
using Server.Service;

namespace Server
{
    public sealed class PreCheckStartServiceRepository
    {
        private readonly ICheckService CheckService;

        public PreCheckStartServiceRepository(ICheckService checkService)
        {
            CheckService = checkService;
        }

        public async void Initialize()
        {
            if (await CheckService.PreCheckRepositoryAsync())
            {
                Console.WriteLine("The database service has successfully started and is ready to go.");
                return;
            }

            Console.WriteLine("There were some problems with the service. The software has ended with an emergency code.");
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }
}