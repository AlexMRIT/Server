namespace Server.Contracts
{
    public sealed class AccountContract
    {
        public readonly int Id;
        public string Login { get; set; }
        public string Password { get; set; }

        public AccountContract(int id)
        {
            Id = id;
        }
    }
}