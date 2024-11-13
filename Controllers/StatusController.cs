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
    public class StatusController : ControllerBase
    {

        private readonly StatusService _statusService;

        public StatusController(StatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("GetAllStatuses")]
        public IResult GetAllStatuses()
        {
            List<Status> results = _statusService.GetAllStatuses();

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No statuses found");
        }

        [HttpGet("GetStatusByName")]
        public IResult GetStatusByName([FromQuery] string name)
        {
            Status result = _statusService.GetStatusByName(name);
            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Status not found");
        }

        [HttpGet("GetStatusViewByName")]
        public IResult GetStatusViewByName([FromQuery] string name)
        {
            Status result = _statusService.GetStatusViewByName(name);
            if (result != null)
                return Results.Ok(result);
            else
                return Results.NotFound("Error: Status not found");
        }

        [HttpGet("GetBuffStatuses")]
        public IResult GetBuffStatuses()
        {
            IList<Status> results = _statusService.GetBuffStatuses();

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No statuses found");
        }

        [HttpGet("GetDebuffStatuses")]
        public IResult GetDebuffStatuses()
        {
            IList<Status> results = _statusService.GetDebuffStatuses();

            if (results != null)
                return Results.Ok(results);
            else
                return Results.NotFound("Error: No statuses found");
        }

    }
}
