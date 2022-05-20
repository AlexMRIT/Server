namespace Server.Contracts
{
    public sealed class CharacterContract
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
    }
}