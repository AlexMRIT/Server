namespace Server.Service.Interface
{
    public interface IBaseSpecification
    {
        public int Strength { get; }
        public int Dextity { get; }
        public int Endurance { get; }
        public int BaseHealth { get; }
    }
}