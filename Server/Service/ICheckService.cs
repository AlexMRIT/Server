using System.Threading.Tasks;

namespace Server.Service
{
    public interface ICheckService
    {
        public Task<bool> PreCheckRepositoryAsync();
    }
}