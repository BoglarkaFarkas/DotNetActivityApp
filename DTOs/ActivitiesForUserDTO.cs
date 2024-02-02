using myappdotnet.Model;

namespace myappdotnet.DTOs;
public class ActivitiesForUserDTO
{
    public DateTime CreatedAt {get; set;}
    public AboutActivityDTO? Activity {get; set;}
}
