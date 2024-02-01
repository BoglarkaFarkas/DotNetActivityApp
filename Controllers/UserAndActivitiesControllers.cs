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
    [Route("activityNameUser")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult PostActivityForUser([FromBody] ActivityNameDTO activityNameDTO)
    {
        try
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Identity.Name;
                var user = context.MyUser.FirstOrDefault(u => u.Email == username);
                var activity = context.Activities.FirstOrDefault(ac => ac.Name == activityNameDTO.Name);
                if (activity == null || user == null)
                {
                    return NotFound();
                }

                var actforuser = new MyUser_Activities();
                actforuser.ActivityId = activity.Id;
                actforuser.UserId = user.Id;
                context.MyUser_Activities.Add(actforuser);
                context.SaveChanges();
                var response = new
                {
                    status = 200,
                    message = $"Activity added for user {user.First_name} {user.Surname}"
                };

                return Ok(response);

            }
            else
            {
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("activityForUser")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
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
                    return NotFound();
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
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }
}
