using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myappdotnet.DTOs;
using myappdotnet.Model;
using myappdotnet.Service;

namespace myappdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly AuthService authService;
    public LocationsController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
    }
    [HttpGet]
    [Route("alllocation")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetAllLocations()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var locations = context.Location.ToList();
                var locationDTOs = locations.Select(location => new AboutLocationDTO
                {
                    NameCity = location.NameCity,
                    ExactLocation = location.ExactLocation
                }).ToList();

                return Ok(locationDTOs);
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
    [Route("allcities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetAllCities()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var locations = context.Location.Select(loc => loc.NameCity).Distinct().ToList();
                var locationDTOs = locations.Select(nameCity => new LocationCityDTO
                {
                    NameCity = nameCity
                }).ToList();

                return Ok(locationDTOs);
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
    [Route("LocationId/{id}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetLocationById(int id)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var location = context.Location.FirstOrDefault(loc => loc.Id == id);
                if (location == null)
                {
                    return NotFound(new { status = 404, message = "Location do not exist" });
                }

                var locationDTO = new AboutLocationDTO
                {
                    NameCity = location.NameCity,
                    ExactLocation = location.ExactLocation
                };

                return Ok(locationDTO);
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
    [Route("LocationCity/{name}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetLocationByCity(string name)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var locations = context.Location.Where(u => u.NameCity == name).ToList();
                if (locations.Count == 0)
                {
                    return NotFound(new { status = 404, message = "Location do not exist" });
                }

                var locationDTOs = locations.Select(location => new AboutLocationDTO
                {
                    NameCity = location.NameCity,
                    ExactLocation = location.ExactLocation
                }).ToList();

                return Ok(locationDTOs);
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
    [Route("locationswithactivities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetLocationWithActivities()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                var locations = context.Location.ToList();
                var locationDTOs = locations.Select(location =>
                {
                    var activities = context.Activities
                        .Where(activity => activity.LocationId == location.Id)
                        .Select(activity => new ActivityDTO
                        {
                            Name = activity.Name,
                            Price = activity.Price,
                            Time = activity.Time
                        }).ToList();

                    return new LocationWithActivitiesDTO
                    {
                        NameCity = location.NameCity,
                        ExactLocation = location.ExactLocation,
                        Activities = activities
                    };
                }).ToList();

                return Ok(locationDTOs);
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
