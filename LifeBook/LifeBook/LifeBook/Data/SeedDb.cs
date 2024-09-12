
using LifeBook.Data.Entities;
using LifeBook.Enums;
using LifeBook.Helpers;

namespace LifeBook.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Aaron", "Gutiérrez", "aaron20770@gmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType,
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result.Succeeded)
                {
                    Console.WriteLine("Usuario creado correctamente.");
                    await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                }
                else
                {
                    Console.WriteLine("Error al crear usuario.");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Usuario ya existe.");
            }

            return user;
        }



        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

    }
}
