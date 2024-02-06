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
    private readonly ActivityService activityService;
    public ActivityController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
        this.activityService = new ActivityService(context);
    }

    [HttpGet]
    [Route("all-activities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<AboutActivityDTO>), StatusCodes.Status200OK)]
    public IActionResult GetAllActivities()
    {

        try
        {
            var activities = activityService.FindAllActivities();
            var activitiesDTO = new List<AboutActivityDTO>();
            foreach (var a in activities)
            {
                var locationDTO = new AboutLocationDTO
                {
                    NameCity = a.Location.NameCity,
                    ExactLocation = a.Location.ExactLocation
                };
                var ActDTO = new AboutActivityDTO
                {
                    Name = a.Name,
                    Price = a.Price,
                    Time = a.Time,
                    Location = locationDTO
                };
                activitiesDTO.Add(ActDTO);
            }
            return Ok(activitiesDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }

    }

    [HttpGet]
    [Route("activities-name")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ActivityNameDTO>), StatusCodes.Status200OK)]
    public IActionResult GetActivitiesName()
    {
        try
        {
            var activities = activityService.AllActivitiesName();
            var activitiesDTO = activities.Select(activities => new ActivityNameDTO
            {
                Name = activities
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
    [Route("activity-id/{id}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(AboutActivityDTO), StatusCodes.Status200OK)]
    public IActionResult GetActivityById(int id)
    {
        try
        {
            var activity = activityService.FindLocationById(id);
            if (activity == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Activity do not exist" });
            }
            var locationDTO = new AboutLocationDTO
            {
                NameCity = activity.Location.NameCity,
                ExactLocation = activity.Location.ExactLocation
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
    [Route("activity-name/{name}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(AboutActivityDTO), StatusCodes.Status200OK)]
    public IActionResult GetActivityByName(string name)
    {
        try
        {
            var activity = activityService.FindLocationByName(name);
            if (activity == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Activity do not exist" });
            }
            var locationDTO = new AboutLocationDTO
            {
                NameCity = activity.Location.NameCity,
                ExactLocation = activity.Location.ExactLocation
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
    [Route("all-prices")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ActivityPriceDTO>), StatusCodes.Status200OK)]
    public IActionResult GetAllPrices()
    {
        try
        {
            var activity = activityService.AllPrices();
            var activityDTO = new List<ActivityPriceDTO>();
            foreach (var a in activity)
            {
                var locationDTO = new ActivityPriceDTO()
                {
                    Price = a
                };
                activityDTO.Add(locationDTO);
            }

            return Ok(activityDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("activity-price/{price}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<AboutActivityDTO>), StatusCodes.Status200OK)]
    public IActionResult GetActivityByPrice(double price)
    {
        try
        {
            var activities = activityService.FindActivitiesByPrice(price);
            if (activities == null || activities.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Activity do not exist" });
            }
            var MyDTO = new List<AboutActivityDTO>();
            foreach (var a in activities)
            {
                var locationDTO = new AboutLocationDTO
                {
                    NameCity = a.Location.NameCity,
                    ExactLocation = a.Location.ExactLocation
                };
                var ActDTO = new AboutActivityDTO
                {
                    Name = a.Name,
                    Price = a.Price,
                    Time = a.Time,
                    Location = locationDTO
                };
                MyDTO.Add(ActDTO);
            }
            return Ok(MyDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }

    }
}
