using Rock_Paper_Scissors.Enum;

namespace Rock_Paper_Scissors.BusinessLogic
{
    public class GameLogic : IGameLogic
    {
        private readonly Random _randomNumber = new Random();
        private static readonly Dictionary<Moves, Moves> WinningMoves = new()
        {
            { Moves.Scissors, Moves.Rock },
            { Moves.Paper, Moves.Scissors },
            { Moves.Rock, Moves.Paper }
        };

        public Game CalculateMove(Moves playerChoice, Game game)
        {
            Moves computerChoice = PredictNextMove(game);
            if (playerChoice == computerChoice)
            {
                game.Draw++;
                game.LastResult = "Tie";
            }
            else if (WinningMoves[computerChoice] == playerChoice)
            {
                game.PlayerScore++;
                game.LastResult = "Player wins!";
            }
            else
            {
                game.ComputerScore++;
                game.LastResult = "Computer wins!";
            }

            return game;
        }

        private Moves GetRandomMove()
        {
            return WinningMoves.Keys.ToArray()[_randomNumber.Next(WinningMoves.Count)];
        }

        private Moves PredictNextMove(Game game)
        {
            Moves mostUsed;
            if (game.Rock >= game.Paper && game.Rock >= game.Scissor)
            {
                mostUsed = Moves.Rock;
            }
            else if (game.Paper >= game.Rock && game.Paper >= game.Scissor)
            {
                mostUsed = Moves.Paper;
            }
            else if (game.Scissor >= game.Rock && game.Scissor >= game.Paper)
            {
                mostUsed = Moves.Scissors;
            }
            else
            {
                mostUsed = GetRandomMove();
            }

            return WinningMoves[mostUsed];
        }
    }
}
