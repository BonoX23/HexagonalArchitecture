using Application;
using Application.Ports;
using Application.Room.Dtos;
using Application.Room.Ports;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<GuestsController> _logger;
        private readonly IRoomManager _roomManager;

        public RoomController(
            ILogger<GuestsController> logger,
            IRoomManager roomManage)
        {
            _logger = logger;
            _roomManager = _roomManager;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var res = await _mediator.Send(request);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }
    }
}