namespace Server.Contracts
{
    public sealed class CharacterContract
    {
        public readonly int Id;
        public int Score { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }

        public CharacterContract(int id)
        {
            Id = id;
        }
    }
}