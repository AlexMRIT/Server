using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Service
{
    public interface IGetAllIdsInitialise
    {
        Task<List<int>> GetPlayerIdsListAsync();

        Task<List<int>> GetAccountIdsListAsync();
    }
}