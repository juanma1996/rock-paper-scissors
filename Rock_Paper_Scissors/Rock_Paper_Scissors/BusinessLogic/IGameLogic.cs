using Rock_Paper_Scissors.Enum;

namespace Rock_Paper_Scissors.BusinessLogic
{
    public interface IGameLogic
    {
        Game CalculateMove(Moves playerChoice, Game game);
    }
}
