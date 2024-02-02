using myappdotnet.Model;
namespace myappdotnet.DTOs;
public class LocationWithActivitiesDTO
{
    public string? NameCity { get; set; }
    public string? ExactLocation { get; set; }
    public ICollection<ActivityDTO>? Activities { get; set; }
}
