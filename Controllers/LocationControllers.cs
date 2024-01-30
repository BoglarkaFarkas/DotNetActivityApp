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
    public IActionResult GetAllLocations()
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

    [HttpGet]
    [Route("allcities")]
    public IActionResult GetAllCities()
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

    [HttpGet]
    [Route("LocationId/{id}")]
    public IActionResult GetLocationById(int id)
    {

        try
        {
            var location = context.Location.FirstOrDefault(loc => loc.Id == id);
            if (location == null)
            {
                return NotFound();
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
    [Route("LocationCity/{name}")]
    public IActionResult GetLocationByCity(string name)
    {
        try
        {
            var locations = context.Location.Where(u => u.NameCity == name).ToList();
            if (locations.Count == 0)
            {
                return NotFound();
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
}
