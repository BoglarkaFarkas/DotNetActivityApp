using BCrypt.Net;
namespace myappdotnet.Service;
using myappdotnet.Model;
public class AuthService
{
    private readonly ApplicationDbContext context;

    public AuthService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public bool AuthenticateUser(string email, string password)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public bool RegisterUser(string email, string password, string surname, string first_name)
    {
        try{
            if (context.MyUser.Any(u => u.Email == email))
            {
                return false;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new MyUser();
            newUser.Surname = surname;
            newUser.First_name = first_name;
            newUser.Password = hashedPassword;
            newUser.Email = email;
            context.MyUser.Add(newUser);
            context.SaveChanges();

            return true; 
        }catch (Exception ex){
            Console.WriteLine(ex.ToString()); 
            return false; 
        }
    }
}
