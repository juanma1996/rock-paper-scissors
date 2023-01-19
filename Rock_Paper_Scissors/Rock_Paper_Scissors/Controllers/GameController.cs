using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Rock_Paper_Scissors.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private DataContext _context;
    private Random _randomNumber = new Random();
    private static readonly Dictionary<int, int> WinningMoves = new Dictionary<int, int>
    {
        {3, 1},
        {2, 3},
        {1, 2}
    };

    public GameController(DataContext dataContext)
    {
        _context = dataContext;
    }

    [HttpGet("[action]")]
    public async Task<Game> Get(Guid id)
    {
        return await _context.Games.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    [HttpPost("[action]")]
    public async Task Start()
    {
        Game game = await GetActiveGame();
        if ( game is null)
        {
            Game newGame = new Game();
            game.Id = new Guid();
            game.IsActive = true;
            _context.Games.Add(game);
            await _context.SaveChangesAsync(); 
        }
        else
        {
            Console.WriteLine("You must finish the game first.");
        }
    }

    [HttpPut("[action]")]
    public async Task<Game> Finish()
    {
        Game game = await GetActiveGame();
        game.IsActive = false;
        await _context.SaveChangesAsync();

        return game;
    }

    [HttpPut("[action]")]
    public async Task<Game> Move(int moveType)
    {
        Game game = await GetActiveGame();
        if (moveType == 1)
        {
            game.Rock++;
        }
        else if (moveType == 2)
        {
            game.Paper++;
        }
        else
        {
            game.Scissor++;
        }
        await CalculateMove(moveType, game);
        await _context.SaveChangesAsync();

        return game;
    }

    private async Task<Game> CalculateMove(int playerChoice, Game game)
    {
        int computerChoice = await PredictNextMove();
        if (playerChoice == computerChoice)
        {
            game.Draw++;
            game.LastResult = "Tie";
        }
        else if (playerChoice == 1 && computerChoice == 3 || playerChoice == 2 && computerChoice == 1 || playerChoice == 3 && computerChoice == 2)
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

    private async Task<Game> GetActiveGame()
    {
        return await _context.Games.Where(x => x.IsActive).FirstOrDefaultAsync();
    }

    private int GetRandomMove()
    {
        return WinningMoves.Keys.ToArray()[_randomNumber.Next(WinningMoves.Count)];
    }

    private async Task<int> PredictNextMove()
    {
        int mostUsed;
        Game game = await GetActiveGame();
        if (game.Rock >= game.Paper && game.Rock >= game.Scissor)
        {
            mostUsed = 1;
        }else if (game.Paper >= game.Rock && game.Paper >= game.Scissor)
        {
            mostUsed = 2;
        } else if (game.Scissor >= game.Rock && game.Scissor >= game.Paper)
        {
            mostUsed = 3;
        }
        else
        {
            mostUsed = GetRandomMove();
        }
        return WinningMoves[mostUsed];
    }
}
