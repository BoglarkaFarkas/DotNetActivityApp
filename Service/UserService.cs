namespace myappdotnet.Service;

using Microsoft.EntityFrameworkCore;
using myappdotnet.Model;
public class UserService
{
    private readonly ApplicationDbContext context;

    public UserService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public List<MyUser> FindAllUsers()
    {
        var users = context.MyUser.ToList();
        return users;
    }

    public MyUser? FindUserByUsername(string username)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == username);
        return user;
    }

    public MyUser? FindUserById(int id)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Id == id);
        return user;
    }

    public bool ChangePwd(string username, string currentPassword, string newPassword)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == username);
        if (user == null)
        {
            return false;
        }
        bool psw = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
        if (psw)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.Password = hashedPassword;
            context.SaveChanges();
        }
        else
        {
            return false;
        }
        return true;
    }

    public bool UserDelete(string username)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == username);
        if (user == null)
        {
            return false;
        }
        int id_user = user.Id;
        var userActivities = context.MyUser_Activities.Where(ua => ua.UserId == id_user).ToList();
        if (userActivities.Any())
        {
            context.MyUser_Activities.RemoveRange(userActivities);
        }
        var userToRemove = context.MyUser.FirstOrDefault(u => u.Id == id_user);
        if (userToRemove != null)
        {
            context.MyUser.Remove(userToRemove);
        }

        context.SaveChanges();
        return true;
    }
}
