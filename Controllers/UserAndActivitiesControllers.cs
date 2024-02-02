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
    public UserAndActivitiesController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (activityNameDTO == null || activityNameDTO.Name == null)
                {
                    return BadRequest(new ErrorResponseDTO{ Status = 400, Error = "Invalid data." });
                }
                var username = HttpContext.User.Identity.Name;
                var user = context.MyUser.FirstOrDefault(u => u.Email == username);
                var activity = context.Activities.FirstOrDefault(ac => ac.Name == activityNameDTO.Name);
                if (activity == null || user == null)
                {
                    return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Activity do not exist." });
                }

                var actforuser = new MyUser_Activities();
                actforuser.ActivityId = activity.Id;
                actforuser.UserId = user.Id;
                context.MyUser_Activities.Add(actforuser);
                context.SaveChanges();
                return Ok(new { message = "Activity added for user." });

            }
            else
            {
                return Unauthorized(new ErrorResponseDTO{ Status = 401, Error = "Unauthorized access." });
            }
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
    [ProducesResponseType(typeof(List<ActivitiesForUserDTO>),StatusCodes.Status200OK)]
    public IActionResult GetActivitiesForUser()
    {
        try
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Identity.Name;
                var user = context.MyUser.FirstOrDefault(u => u.Email == username);
                if (user == null)
                {
                    return NotFound(new ErrorResponseDTO{ Status = 404, Error = "User do not exist" });
                }
                var act = context.MyUser_Activities.Where(ac => ac.UserId == user.Id).ToList();
                var activityDTOs = new List<ActivitiesForUserDTO>();
                foreach (var action in act)
                {
                    var activities = context.Activities.FirstOrDefault(a => a.Id == action.ActivityId);
                    if (activities != null)
                    {
                        var location = context.Location.FirstOrDefault(l => l.Id == activities.LocationId);
                        if (location != null)
                        {
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
                            activityDTOs.Add(activitiesDTO);
                        }

                    }

                }
                return Ok(activityDTOs);

            }
            else
            {
                return Unauthorized(new ErrorResponseDTO{ Status = 401, Error = "Unauthorized access." });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }
}
