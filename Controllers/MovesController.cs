using CassetteBeastsAPI.Models;
using CassetteBeastsAPI.Services;
using CassetteBeastsAPI.Utilities;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CassetteBeastsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovesController : ControllerBase
    {
        private readonly MovesService _movesService;

        public MovesController(MovesService movesService)
        {
            _movesService = movesService;
        }


        [HttpGet("GetAllMoveListViews")]
        public IResult GetAllMoveListViews()
        {
            IList<MoveListDetailView> results = _movesService.GetAllMoveListViews();

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");
        }

        [HttpGet("GetAllMoves")]
        public IResult GetAllMoves()
        {
            IList<Move> results = _movesService.GetAllMoves();

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");
        }

        [HttpGet("GetMoveByName")]
        public IResult GetMoveByName([FromQuery] string name)
        {
            Move result = _movesService.GetMoveByName(name);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Move not found");

        }
        [HttpGet("GetMoveByKey")]
        public IResult GetMoveByKey([FromQuery] string key)
        {
            Move result = _movesService.GetMoveByKey(key);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Move not found");

        }


        [HttpGet("GetMovesByElementalType")]
        public IResult GetMovesByElementalType([FromQuery] string elementalType)
        {
            IList<Move> results = _movesService.GetMovesByElementalType(elementalType);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }

        [HttpGet("GetMovesByCategory")]
        public IResult GetMovesByCategory([FromQuery] string category)
        {
            IList<Move> results = _movesService.GetMovesByCategory(category);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }

        [HttpGet("GetMovesByTag")]
        public IResult GetMovesByTag([FromQuery] string tag)
        {
            IList<Move> results = _movesService.GetMovesByTag(tag);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }

        [HttpGet("GetMovesByStatusEffect")]
        public IResult GetMovesByStatusEffect([FromQuery] string status)
        {
            IList<Move> results = _movesService.GetMovesByStatusEffect(status);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }

        [HttpGet("GetMeleeMoves")]
        public IResult GetMeleeMoves()
        {
            IList<Move> results = _movesService.GetMovesByPhysicality("MELEE");

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }

        [HttpGet("GetRangedMoves")]
        public IResult GetRangedMoves()
        {
            IList<Move> results = _movesService.GetMovesByPhysicality("RANGED");

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No moves found");

        }
    }
}
