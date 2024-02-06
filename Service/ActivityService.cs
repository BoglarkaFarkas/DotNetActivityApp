namespace myappdotnet.Service;

using Microsoft.EntityFrameworkCore;
using myappdotnet.Model;
public class ActivityService
{
    private readonly ApplicationDbContext context;

    public ActivityService(ApplicationDbContext context)
    {
        this.context = context;
    }


    public List<string> AllActivitiesName()
    {
        var activities = context.Activities.Select(ac => ac.Name).ToList();
        return activities;
    }
    public Activities? FindLocationById(int id)
    {
        var activity = context.Activities.FirstOrDefault(ac => ac.Id == id);
        if (activity == null)
        {
            return null;
        }
        var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
        if (location == null)
        {
            return null;
        }
        return activity;
    }

    public Activities? FindLocationByName(string name)
    {
        var activity = context.Activities.FirstOrDefault(ac => ac.Name == name);
        if (activity == null)
        {
            return null;
        }
        var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
        if (location == null)
        {
            return null;
        }
        return activity;
    }

    public List<double> AllPrices()
    {
        var activities = context.Activities.Select(ac => ac.Price).Distinct().ToList();
        return activities;
    }

    public List<Activities>? FindActivitiesByPrice(double price)
    {
        var activities = context.Activities
                            .Include(a => a.Location)
                            .Where(a => a.Price == price)
                            .ToList();
        return activities;
    }

    public List<Activities> FindAllActivities()
    {
        var activities = context.Activities
                            .Include(a => a.Location)
                            .ToList();
        return activities;
    }
}
