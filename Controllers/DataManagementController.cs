//using CassetteBeastsAPI.Models;
//using CassetteBeastsAPI.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.FeatureManagement.Mvc;

//namespace CassetteBeastsAPI.Controllers
//{
//    [FeatureGate("DevOnly")]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DataManagementController : Controller
//    {

//        private readonly DataManagementService _dataManagementService;

//        public DataManagementController(DataManagementService dataManagementService)
//        {
//            _dataManagementService = dataManagementService;
//        }

//        [HttpGet("InjestSpeciesJson")]
//        public async Task<IResult> InjestSpeciesJson()
//        {

//            IList<Species> result = await _dataManagementService.InjestSpecies();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Species JSON injest failed");
//        }

//        [HttpGet("InjestElementalTypesJson")]
//        public async Task<IResult> InjestElementalTypesJson()
//        {

//            IList<ElementalType> result = await _dataManagementService.InjestElementalTypes();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Elemental Types JSON injest failed");
//        }

//        [HttpGet("InjestTypeInteractionsJson")]
//        public async Task<IResult> InjestTypeInteractionsJson()
//        {

//            List<Interaction> result = await _dataManagementService.InjestTypeInteractions();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Elemental Types JSON injest failed");
//        }

//        [HttpGet("InjestMovesJson")]
//        public async Task<IResult> InjestMovesJson()
//        {

//            IList<Move> result = await _dataManagementService.InjestMoves();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Moves JSON injest failed");
//        }

//        [HttpGet("InjestStatusJson")]
//        public async Task<IResult> InjestStatusJson()
//        {

//            IList<Status> result = await _dataManagementService.InjestStatus();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Status JSON injest failed");
//        }

//        [HttpGet("InjestFusionPartSprites")]
//        public async Task<IResult> InjestFusionPartSprites()
//        {

//            IList<FusionPartSprite> result = await _dataManagementService.InjestFusionParts();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Fusion Part JSON injest failed");
//        }

//        [HttpGet("InjestFusionComponents")]
//        public async Task<IResult> InjestFusionComponents()
//        {

//            IList<FusionComponent> result = await _dataManagementService.InjestFusionComponents();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Fusion Component JSON injest failed");
//        }

//        [HttpGet("InjestSpeciesAnimations")]
//        public async Task<IResult> InjestSpeciesAnimations()
//        {

//            IList<SpeciesSprite> result = await _dataManagementService.InjestSpeciesAnimations();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("Error: Sprite JSON injest failed");
//        }

//        [HttpGet("TestDataMgmt")]
//        public async Task<IResult> TestDataMgmt()
//        {
//            string result = _dataManagementService.TestDataMgmt();

//            if (result != null)
//                return Results.Ok(result);
//            else
//                return Results.NotFound("failed");

//        }
//    }
//}
