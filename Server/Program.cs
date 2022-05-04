using System;
using Server.Utilite;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    internal sealed class Program
    {
        private static void Main()
        {
            try
            {
                ServiceCollection serviceDescriptors = new ServiceCollection();
                BuildServiceCollection.Build(serviceDescriptors);

                ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
                Task.Factory.StartNew(serviceProvider.GetService<GameServer>().GameServerStart);

                Process.GetCurrentProcess().WaitForExit();
            } 
            catch (Exception exception)
            {
                ExceptionHandler.Execute(exception, nameof(Main));
            }

            Console.ReadLine();
        }
    }
}
