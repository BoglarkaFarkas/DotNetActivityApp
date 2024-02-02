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
    [Route("all-activities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<AboutActivityDTO>), StatusCodes.Status200OK)]
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
    [Route("activities-name")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ActivityNameDTO>), StatusCodes.Status200OK)]
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
            var activity = context.Activities.FirstOrDefault(ac => ac.Id == id);
            if (activity == null)
            {
                return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Activity do not exist" });
            }
            var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
            if (location == null)
            {
                return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Location do not exist" });
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
            var activity = context.Activities.FirstOrDefault(ac => ac.Name == name);
            if (activity == null)
            {
                return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Activity do not exist" });
            }
            var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
            if (location == null)
            {
                return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Location do not exist" });
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
    [Route("all-prices")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ActivityPriceDTO>), StatusCodes.Status200OK)]
    public IActionResult GetAllPrices()
    {

        try
        {
            var activity = context.Activities.Select(ac => ac.Price).Distinct().ToList();
            var activityDTO = activity.Select(price => new ActivityPriceDTO
            {
                Price = price
            }).ToList();

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
            var activity = context.Activities.Where(a => a.Price == price).ToList();
            if (activity.Count == 0)
            {
                return NotFound(new ErrorResponseDTO{ Status = 404, Error = "Activity do not exist" });
            }

            var activitiesDTO = activity.Select(activity =>
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
}
