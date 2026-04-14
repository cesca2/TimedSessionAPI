using Microsoft.AspNetCore.Mvc;
using SessionAPI.Services;
using SQLitePCL;

namespace SessionAPI.Controllers
{
    // specifies routing pattern for the controller [controller] is replaced with name of controller minus Controller suffix
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController(ISessionService sessionService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;

        [HttpGet]
        public ActionResult<List<Session>> GetAllSessions([FromQuery] PaginationParams paginationParams)
        {
            try{return Ok(_sessionService.GetAllRecords(paginationParams));}
            catch(ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }}
        

        [HttpGet("{id}")]
        public ActionResult<Session> GetSessionById(Guid id)
        {
        try{
            var result = _sessionService.GetRecord(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
        }
        catch(ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
        }}
        
    

        [HttpPost]
        public ActionResult<Session> CreateSession(Session session)
        {
            var newsession = _sessionService.CreateSession(session);
            try
            {
                return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, newsession);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<Session> DeleteSession(Guid id)
        {
            try
            {
                var result = _sessionService.DeleteSession(id);
                if (result == null)
                {
                    return NotFound($"Could not find record with ID {id}");
                }

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });

            }

        }

        [HttpPut("{id}")]
        public ActionResult<Session> UpdateSession(Guid id, [FromBody] Session updatedSession)
        {
            try
            {

                if (id != updatedSession.Id)
                {
                    return BadRequest(new
                    {
                        errors = new[]
                        {
                        "Id is invalid, must match value in new Session"
                    }
                    });
                }
                var result = _sessionService.UpdateSession(id, updatedSession);

                if (result == null)
                {
                    return NotFound();
                }

                return Created("", result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });

            }
        }


    }
}