using System.Collections.Generic;
using System.Linq;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Repositorios
{
    public static class UserRepositorio
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>();

            users.Add(new User
            {
                Id = 1, 
                Username = "link", 
                Password = "link", 
                Role = "adm"
            });

            users.Add(new User
            {
                Id = 2,
                Username = "tradutor",
                Password = "tradutor",
                Role = "tradutor"
            });


            return users
                .Where(x =>
                    x.Username.ToLower() == username.ToLower() &&
                    x.Password == password)
                .FirstOrDefault();
        }
    }
}
