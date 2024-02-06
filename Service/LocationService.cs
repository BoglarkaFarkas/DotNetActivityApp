namespace myappdotnet.Service;

using Microsoft.EntityFrameworkCore;
using myappdotnet.Model;
public class LocationService
{
    private readonly ApplicationDbContext context;

    public LocationService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public List<Location> AllLocations()
    {
        var locations = context.Location.ToList();
        return locations;
    }

    public List<string> AllCityNames()
    {
        var locations = context.Location.Select(loc => loc.NameCity).Distinct().ToList();
        return locations;
    }

    public Location? FindLocationById(int id)
    {
        var location = context.Location.FirstOrDefault(loc => loc.Id == id);
        return location;
    }

    public List<Location>? FindLocationByCityName(string name)
    {
        var locations = context.Location.Where(u => u.NameCity == name).ToList();
        if (locations.Count == 0)
        {
            return null;
        }
        return locations;
    }
    public List<Location> AllLocationWithActivities()
    {
        var locations = context.Location.ToList();
        var mylocation = locations.Select(location =>
            {
                var activities = context.Activities
                    .Where(activity => activity.LocationId == location.Id)
                    .Select(activity => new Activities
                    {
                        Name = activity.Name,
                        Price = activity.Price,
                        Time = activity.Time
                    }).ToList();

                return new Location
                {
                    NameCity = location.NameCity,
                    ExactLocation = location.ExactLocation,
                    Activities = activities
                };
            }).ToList();
        return mylocation;
    }
}
