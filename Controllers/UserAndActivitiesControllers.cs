using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myappdotnet.DTOs;
using myappdotnet.Model;
using myappdotnet.Service;

namespace myappdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class UserAndActivitiesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly AuthService authService;
    private readonly UserAndActivitiesService userAndActivitiesService;

    public UserAndActivitiesController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
        this.userAndActivitiesService = new UserAndActivitiesService(context);
    }

    [HttpPost]
    [Route("activity-name-user")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult PostActivityForUser([FromBody] ActivityNameDTO activityNameDTO)
    {
        try
        {
            if (activityNameDTO == null || activityNameDTO.Name == null)
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid data." });
            }
            var username = HttpContext.User.Identity.Name;

            if (username == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist." });
            }
            bool is_activity_added = userAndActivitiesService.SaveUserActivity(username, activityNameDTO.Name);
            if (!is_activity_added)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Activity do not exist." });
            }
            return Ok(new { message = "Activity added for user." });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("activity-for-user")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ActivitiesForUserDTO>), StatusCodes.Status200OK)]
    public IActionResult GetActivitiesForUser()
    {
        try
        {
            var username = HttpContext.User.Identity.Name;
            if (username == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
            }
            var my_activities = userAndActivitiesService.AboutActivitiesUser(username);
            if (my_activities == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "User or Location or Activity do not exist" });
            }
            var activityDTOs = new LinkedList<ActivitiesForUserDTO>();
            foreach (var action in my_activities)
            {
                var activities = action.Activity;
                var location = activities.Location;
                var locationDTO = new AboutLocationDTO
                {
                    NameCity = location.NameCity,
                    ExactLocation = location.ExactLocation
                };
                var activityDTO = new AboutActivityDTO
                {
                    Name = activities.Name,
                    Price = activities.Price,
                    Time = activities.Time,
                    Location = locationDTO
                };
                var activitiesDTO = new ActivitiesForUserDTO
                {
                    CreatedAt = action.CreatedAt,
                    Activity = activityDTO
                };
                
                activityDTOs.AddLast(activitiesDTO);
            }

            return Ok(activityDTOs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }
}
