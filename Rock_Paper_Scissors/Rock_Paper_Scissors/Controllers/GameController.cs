using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rock_Paper_Scissors.BusinessLogic;
using Rock_Paper_Scissors.Enum;

namespace Rock_Paper_Scissors.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IGameLogic _gameLogic;


    public GameController(DataContext dataContext, IGameLogic gameLogic)
    {
        _context = dataContext;
        _gameLogic = gameLogic;
    }

    [HttpGet(Name = "GetAll")]
    public async Task<List<Game>> Get()
    {
        return await _context.Games.ToListAsync();
    }

    [HttpGet("{id}", Name = "Get")]
    public async Task<Game> Get(Guid id)
    {
        return await _context.Games.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    [HttpPost("[action]")]
    public async Task<Game> Start()
    { 
            Game game = new Game();
            game.Id = new Guid();
            game.IsActive = true;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return game;
    }

    [HttpPut("[action]/{id}")]
    public async Task<Game> Finish(Guid id)
    {
        Game game = await _context.Games.Where(x => x.Id == id).FirstOrDefaultAsync();
        game.IsActive = false;
        await _context.SaveChangesAsync();

        return game;
    }

    [HttpPut("[action]/{id}")]
    public async Task<Game> Move(Guid id, Moves moveType)
    {
        Game game = await _context.Games.Where(x => x.Id == id).FirstOrDefaultAsync();
        switch (moveType)
        {
            case Moves.Rock:
                game.Rock++;
                break;
            case Moves.Paper:
                game.Paper++;
                break;
            case Moves.Scissors:
                game.Scissor++;
                break;
        }

        _gameLogic.CalculateMove(moveType, game);
        await _context.SaveChangesAsync();

        return game;
    }
}
