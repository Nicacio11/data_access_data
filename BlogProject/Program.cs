using System;
using BlogProject.Models;
using BlogProject.Repositories;
using Microsoft.Data.SqlClient;

namespace BlogProject
{
    class Program
    {
        private const string connectionString = "Server=localhost,1433;Database=blog;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var user = new User()
            {
                Email = "s2da@example.com",
                Image = "http://example.com",
                Name = "sa",
                PasswordHash = "password",
                Slug = "ssd2",
                Bio = "sa",
                Id = 0
            };

            var repository = new UserRepository(connection);
            repository.Create(user);
            connection.Close();
        }

    }
}
