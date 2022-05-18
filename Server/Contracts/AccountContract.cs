namespace Server.Contracts
{
    public sealed class AccountContract
    {
        public int Id;
        public string Login { get; set; }
        public string Password { get; set; }
    }
}