using CassetteBeastsAPI.Models;
using CassetteBeastsAPI.Services;
using CassetteBeastsAPI.Utilities;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CassetteBeastsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElementalTypesController : ControllerBase
    {
        private readonly ElementalTypesService _elementalTypesService;

        public ElementalTypesController(ElementalTypesService elementalTypesService)
        {
            _elementalTypesService = elementalTypesService;
        }

        [HttpGet("GetAllElementalTypes")]
        public IResult GetAllElementalTypes()
        {
            IList<ElementalType> results = _elementalTypesService.GetAllElementalTypes();

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No Elemental Types found");
        }

        [HttpGet("GetElementalTypesDropdown")]
        public IResult GetElementalTypesDropdown()
        {
            IList<string> results = _elementalTypesService.GetElementalTypesDropdown();

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No Elemental Types found");
        }

        [HttpGet("GetElementalTypeByName")]
        public IResult GetElementalTypeByName([FromQuery] string name)
        {
            if (!Global.ElementalTypeList.Contains(name.ToLower()))
            {
                return Results.BadRequest("Invalid elemental type");
            }

            ElementalType result = _elementalTypesService.GetElementalTypeByName(name);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Elemental Type not found");

        }

        [HttpGet("GetElementalTypeViewByName")]
        public IResult GetElementalTypeViewByName([FromQuery] string name)
        {
            if (!Global.ElementalTypeList.Contains(name.ToLower()))
            {
                return Results.BadRequest("Invalid elemental type");
            }

            ElementalTypePageView result = _elementalTypesService.GetElementalTypeViewByName(name);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Elemental Type not found");

        }

    }
}
