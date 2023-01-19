namespace Rock_Paper_Scissors
{
    public class Game
    {
        public Guid Id { get; set; }
        public int PlayerScore { get; set; }
        public int ComputerScore { get; set; }
        public int Draw { get; set; }
        public int Rock { get; set; }
        public int Paper { get; set; }
        public int Scissor { get; set; }
        public bool IsActive { get; set; }
        public string LastResult { get; set; }
    }
}