namespace Server.Template
{
    public sealed class BaseSpecificationForCharacter
    {
        public int Strenght { get; set; }
        public int Dextity { get; set; }
        public int Endurance { get; set; }

        public BaseSpecificationForCharacter()
        {
            Strenght = 1;
            Dextity = 1;
            Endurance = 1;
        }
    }
}