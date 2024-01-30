using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myappdotnet.DTOs;
using myappdotnet.Model;
using myappdotnet.Service;

namespace myappdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly AuthService authService;
    public ActivityController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
    }

    [HttpGet]
    [Route("allactivities")]
    public IActionResult GetAllActivities()
    {
        try
        {
            var activities = context.Activities.ToList();
            var activitiesDTO = activities.Select(activity =>
            {
                var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
                if (location != null)
                {
                    var locationDTO = new AboutLocationDTO
                    {
                        NameCity = location.NameCity,
                        ExactLocation = location.ExactLocation
                    };

                    return new AboutActivityDTO
                    {
                        Name = activity.Name,
                        Price = activity.Price,
                        Time = activity.Time,
                        Location = locationDTO
                    };
                }
                else
                {
                    return null;
                }
            }).Where(dto => dto != null).ToList();

            return Ok(activitiesDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("activitiesname")]
    public IActionResult GetActivitiesName()
    {
        try
        {
            var activities = context.Activities.ToList();
            var activitiesDTO = activities.Select(activities => new ActivityNameDTO
            {
                Name = activities.Name
            }).ToList();

            return Ok(activitiesDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("ActivityId/{id}")]
    public IActionResult GetActivityById(int id)
    {

        try
        {
            var activity = context.Activities.FirstOrDefault(ac => ac.Id == id);
            if (activity == null)
            {
                return NotFound();
            }
            var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
            if (location == null)
            {
                return NotFound();
            }
            var locationDTO = new AboutLocationDTO
            {
                NameCity = location.NameCity,
                ExactLocation = location.ExactLocation
            };

            var activityDTO = new AboutActivityDTO
            {
                Name = activity.Name,
                Price = activity.Price,
                Time = activity.Time,
                Location = locationDTO
            };

            return Ok(activityDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("ActivityName/{name}")]
    public IActionResult GetActivityByName(string name)
    {

        try
        {
            var activity = context.Activities.FirstOrDefault(ac => ac.Name == name);
            if (activity == null)
            {
                return NotFound();
            }
            var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
            if (location == null)
            {
                return NotFound();
            }
            var locationDTO = new AboutLocationDTO
            {
                NameCity = location.NameCity,
                ExactLocation = location.ExactLocation
            };
            var activityDTO = new AboutActivityDTO
            {
                Name = activity.Name,
                Price = activity.Price,
                Time = activity.Time,
                Location = locationDTO
            };

            return Ok(activityDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }
}
