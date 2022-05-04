using Server.Contracts;
using System.Threading.Tasks;

namespace Server.Service
{
    public interface IAccountServices
    {
        Task<AccountContract> GetAccountByLoginAsync(string login);

        Task<AccountContract> CreateAccountAsync(string login, string password);

        Task<bool> CheckIfAccountIsCorrectAsync(string login, string password);
    }
}