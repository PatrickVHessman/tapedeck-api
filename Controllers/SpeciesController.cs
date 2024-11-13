using CassetteBeastsAPI.Models;
using CassetteBeastsAPI.Services;
using CassetteBeastsAPI.Utilities;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.FeatureManagement.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CassetteBeastsAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly SpeciesService _speciesService;

        public SpeciesController(SpeciesService speciesService)
        {
            _speciesService = speciesService;
        }


        [HttpGet("GetAllSpecies")]
        public IResult GetAllSpecies()
        {
            IList<Species> results = _speciesService.GetAllSpecies();

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }

        [HttpGet("GetAllSpeciesNameList")]
        public IResult GetAllSpeciesNameList()
        {
            IList<string> results = _speciesService.GetAllSpeciesNameList();

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }

        [HttpGet("GetSpeciesNameDropdownList")]
        public IResult GetSpeciesNameDropdownList([FromQuery] bool inclSec = false, bool inclDlc = false)
        {
            IList<DropdownMenuItem> results = _speciesService.GetSpeciesNameDropdownList(inclSec, inclDlc);

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }
        [HttpGet("GetSpeciesListItems")]
        public IResult GetSpeciesListItems([FromQuery] bool inclSec = false, bool inclDlc = false)
        {
            IList<SpeciesListItem> results = _speciesService.GetSpeciesListItems(inclSec, inclDlc);

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }


        [HttpGet("GetSpeciesByName")]
        public IResult GetSpeciesByName([FromQuery] string name)
        {
            Species result = _speciesService.GetSpeciesByName(name);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Species not found");

        }

        [HttpGet("GetSpeciesViewByName")]
        public IResult GetSpeciesViewByName([FromQuery] string name)
        {
            SpeciesView result = _speciesService.GetSpeciesViewByName(name);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Species not found");

        }

        [HttpGet("GetSpeciesBootlegViewByName")]
        public IResult GetSpeciesBootlegViewByName([FromQuery] string name, string bootleg)
        {
            BootlegSpeciesView result = _speciesService.GetSpeciesBootlegViewByName(name, bootleg);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Species not found");

        }

        [HttpGet("GetSpeciesByBestiaryIndex")]
        public IResult GetSpeciesByBestiaryIndex([FromQuery] int index)
        {

            // Returns a list in the case of the the query being -1, which all of the DLC monsters share
            List<Species> results = _speciesService.GetSpeciesByBestiaryIndex(index);

                if (results.Count > 0)
            {
                if (results.Count == 1)
                {
                    return Results.Ok(results[0]);
                } else
                {
                    return Results.Ok(results);
                }
            }
                else
            {
                return Results.NotFound("Error: Species not found");
            }
                    
            
        }
        [HttpGet("GetSpeciesByElementalType")]
        public IResult GetSpeciesByElementalType([FromQuery] string type)
        {

            if (!Global.ElementalTypeList.Contains(type.ToLower()))
            {
                return Results.BadRequest("Invalid elemental type");
            }

            List<Species> results = _speciesService.GetSpeciesByElementalType(type);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }

        [HttpGet("GetSpeciesByMove")]
        public IResult GetSpeciesByMove([FromQuery] string move)
        {
            List<Species> results = _speciesService.GetSpeciesByMove(move);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }

        [HttpGet("GetSpeciesViewByMove")]
        public IResult GetSpeciesViewByMove([FromQuery] string move)
        {
            List<MoveSpeciesListItem> results = _speciesService.GetSpeciesViewByMove(move);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No species found");
        }

        [HttpGet("GetSpeciesByHabitat")]
        public IResult GetSpeciesByHabitat([FromQuery] string habitat)
        {
            List<Species> results = _speciesService.GetSpeciesByHabitat(habitat);

            if (results.Count > 0)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: Species not found");
        }

        [HttpGet("GetBootlegSpecies")]
        public IResult GetBootlegSpecies([FromQuery] string name, string elementalType)
        {
            if (!Global.ElementalTypeList.Contains(elementalType.ToLower()))
            {
                return Results.BadRequest("Invalid elemental type");
            }

            Species result = _speciesService.GetBootlegSpecies(name,elementalType);

            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Species not found");

        }
    }
}
