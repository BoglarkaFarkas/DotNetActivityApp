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
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetAllActivities()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }

    [HttpGet]
    [Route("activitiesname")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetActivitiesName()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }

    [HttpGet]
    [Route("ActivityId/{id}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetActivityById(int id)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var activity = context.Activities.FirstOrDefault(ac => ac.Id == id);
                if (activity == null)
                {
                    return NotFound(new { status = 404, message = "Activity do not exist" });
                }
                var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
                if (location == null)
                {
                    return NotFound(new { status = 404, message = "Location do not exist" });
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }

    [HttpGet]
    [Route("ActivityName/{name}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetActivityByName(string name)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var activity = context.Activities.FirstOrDefault(ac => ac.Name == name);
                if (activity == null)
                {
                    return NotFound(new { status = 404, message = "Activity do not exist" });
                }
                var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
                if (location == null)
                {
                    return NotFound(new { status = 404, message = "Location do not exist" });
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }

    [HttpGet]
    [Route("allprices")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetAllPrices()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }

    [HttpGet]
    [Route("activityprice/{price}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetActivityByPrice(double price)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var activity = context.Activities.Where(a => a.Price == price).ToList();
                if (activity.Count == 0)
                {
                    return NotFound(new { status = 404, message = "Activity do not exist" });
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
        else
        {
            return Unauthorized(new { status = 401, message = "Unauthorized access." });
        }
    }
}
