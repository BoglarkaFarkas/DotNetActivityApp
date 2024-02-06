using System.ComponentModel;
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
    private readonly LocationService locationService;
    public LocationsController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context);
        this.locationService = new LocationService(context);
    }
    [HttpGet]
    [Route("all-location")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<AboutLocationDTO>), StatusCodes.Status200OK)]
    public IActionResult GetAllLocations()
    {

        try
        {
            var loc = locationService.AllLocations();
            var locationDTOs = new List<AboutLocationDTO>();
            foreach (var l in loc)
            {
                var locationDTO = new AboutLocationDTO()
                {
                    NameCity = l.NameCity,
                    ExactLocation = l.ExactLocation
                };
                locationDTOs.Add(locationDTO);
            }

            return Ok(locationDTOs);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }

    }

    [HttpGet]
    [Route("all-cities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<LocationCityDTO>), StatusCodes.Status200OK)]
    public IActionResult GetAllCities()
    {
        try
        {
            var loc = locationService.AllCityNames();
            var locationDTOs = new List<LocationCityDTO>();
            foreach (var l in loc)
            {
                var locationDTO = new LocationCityDTO()
                {
                    NameCity = l
                };
                locationDTOs.Add(locationDTO);
            }


            return Ok(locationDTOs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("location-id/{id}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(AboutLocationDTO), StatusCodes.Status200OK)]
    public IActionResult GetLocationById(int id)
    {
        try
        {
            var location = locationService.FindLocationById(id);
            if (location == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Location do not exist" });
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

    [HttpGet]
    [Route("location-city/{name}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<AboutLocationDTO>), StatusCodes.Status200OK)]
    public IActionResult GetLocationByCity(string name)
    {

        try
        {
            var locations = locationService.FindLocationByCityName(name);
            if (locations == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "Location do not exist" });
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

    [HttpGet]
    [Route("locations-with-activities")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<LocationWithActivitiesDTO>), StatusCodes.Status200OK)]
    public IActionResult GetLocationWithActivities()
    {

        try
        {
            var locations = locationService.AllLocationWithActivities();
            var locationDTOs = locations.Select(location =>
        {
            var activityDTOs = location.Activities.Select(activity =>
                new ActivityDTO
                {
                    Name = activity.Name,
                    Price = activity.Price,
                    Time = activity.Time
                }).ToList();

            return new LocationWithActivitiesDTO
            {
                NameCity = location.NameCity,
                ExactLocation = location.ExactLocation,
                Activities = activityDTOs
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

}
