using CassetteBeastsAPI.Models;
using CassetteBeastsAPI.Services;
using CassetteBeastsAPI.Utilities;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CassetteBeastsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FusionController : ControllerBase
    {

        private readonly FusionService _fusionService;

        public FusionController(FusionService fusionService)
        {
            _fusionService = fusionService;
        }

        [HttpGet("FusionTest")]
        public IResult FusionTest()
        {
            FusionMonster result = _fusionService.GetFusion("Pombomb", "Traffikrab",0,"none", "none");
            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Status not found");
        }


        [HttpGet("GetFusion")]
        public IResult GetFusion([FromQuery] string monster1, string monster2, ulong seed = 0, string mon1bootleg = "none", string mon2bootleg = "none")
        {
            FusionMonster result = _fusionService.GetFusion(monster1,monster2,seed,mon1bootleg,mon2bootleg);
            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Status not found");
        }

        

    }
}
