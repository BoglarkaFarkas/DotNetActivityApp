namespace myappdotnet.Service;

using Microsoft.EntityFrameworkCore;
using myappdotnet.Model;
public class UserAndActivitiesService
{
    private readonly ApplicationDbContext context;

    public UserAndActivitiesService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public bool SaveUserActivity(string username, String name)
    {
        var myUser = context.MyUser.FirstOrDefault(u => u.Email == username);
        var activities = context.Activities.FirstOrDefault(ac => ac.Name == name);
        if (activities == null || myUser == null)
        {
            return false;
        }
        var actforuser = new MyUser_Activities();
        actforuser.ActivityId = activities.Id;
        actforuser.UserId = myUser.Id;
        context.MyUser_Activities.Add(actforuser);
        context.SaveChanges();
        return true;
    }

    public LinkedList<MyUser_Activities>? AboutActivitiesUser(string username)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == username);
        if (user == null)
        {
            return null;
        }
        var act = context.MyUser_Activities.Where(ac => ac.UserId == user.Id)
                                    .Include(ac => ac.Activity)
                                    .ToList();
        var activities = new LinkedList<MyUser_Activities>();
        foreach (var action in act)
        {
            var activity = context.Activities.FirstOrDefault(a => a.Id == action.ActivityId);
            if (activity != null)
            {
                var location = context.Location.FirstOrDefault(l => l.Id == activity.LocationId);
                if (location != null)
                {
                    var mylocation = new Location
                    {
                        NameCity = location.NameCity,
                        ExactLocation = location.ExactLocation
                    };

                    var myactivity = new Activities
                    {
                        Name = activity.Name,
                        Price = activity.Price,
                        Time = activity.Time,
                        Location = mylocation
                    };
                    var myactivities = new MyUser_Activities
                    {
                        CreatedAt = action.CreatedAt,
                        Activity = myactivity
                    };
                    activities.AddLast(myactivities);
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }
        return activities;
    }
}
