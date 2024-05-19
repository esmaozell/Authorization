

namespace Login_Register.Models
{
    public class ApiUsers
    {
        public static List<User> Users = new()
        {
            new User {Id = 1, UserName ="Esma", Password = "123456", Role = "Admin"},
            new User {Id = 2, UserName ="Elif", Password = "123456", Role = "Customer"}

        };
    }
}
