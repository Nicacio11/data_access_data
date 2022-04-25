using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DataAccessDapper.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True";
            // QueryComAdoNet(connectionString);
            QueryComDapper(connectionString);
        }

        static void QueryComAdoNet(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using( var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "Select id, title from CATEGORY";

                    var reader = command.ExecuteReader();
                    while(reader.Read()){
                        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }
                }

            }
        }

        static void QueryComDapper(string connectionString)
        {

            using (var connection = new SqlConnection(connectionString))
            {

                //UpdateCategory(connection);
                //CreateManyCategory(connection);
                //ListCategories(connection);
                //ExecuteProcedure(connection);
                //ExecuteScalarCategory(connection);
                //OneToOne(connection);
                //OneToMany(connection);
                //QueryMultiple(connection);
                //SelectIn(connection);
                //Like(connection, "api");
                Transaction(connection);

            }
        }
        static void ListCategories(SqlConnection connection)
        {
             var categories = connection.Query<Category>("Select [id], [title] from [CATEGORY]");

                foreach(var category in categories){
                     Console.WriteLine($"{category.Id} - {category.Title}");
                }
        }
        static void ReadView(SqlConnection connection)
        {
             var categories = connection.Query<Category>("Select [id], [title] from [vwCategory]");

                foreach(var category in categories){
                     Console.WriteLine($"{category.Id} - {category.Title}");
                }
        }
        static void CreateCategory(SqlConnection connection)
        {
            var insert = 
            @"INSERT INTO CATEGORY VALUES(@Id, 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert = new Category()
            {
                Id = Guid.NewGuid(),
                Title = "Amazon WS",
                Url = "Amazon",
                Description = "Serviços WS",
                Order = 8,
                Summary = "Aws CLOUD"
            };

            var rows = connection.Execute(insert, categoryInsert);
        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [CATEGORY] SET [Title] = @title where [Id]=@id";
            var rows = connection.Execute(updateQuery, new { title = "FrontEnd 2022", Id = new Guid("b4c5af73-7e02-4ff7-951c-f69ee1729cac")});
            Console.WriteLine($"{rows} registro - atualizado");
        }
        static void CreateManyCategory(SqlConnection connection)
        {
            var insert = 
            @"INSERT INTO CATEGORY VALUES(@Id, 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert =  new List<Category>()
            {
               new Category() 
               {
                    Id = Guid.NewGuid(),
                    Title = "Amazon WS NOVA",
                    Url = "Amazon",
                    Description = "Serviços WS",
                    Order = 8,
                    Summary = "Aws CLOUD"
                },
                new Category() 
               {
                    Id = Guid.NewGuid(),
                    Title = "Amazon WS NOVA 2",
                    Url = "Amazon",
                    Description = "Serviços WS",
                    Order = 8,
                    Summary = "Aws CLOUD"
                }
            };
            

            var rows = connection.Execute(insert, categoryInsert);

            Console.WriteLine($"{rows} registro - atualizado");

        }
    
        static void ExecuteProcedure(SqlConnection connection)
        {
            var sql = "[spDeleteStudent]";
            var @params = new {StudentId = "c28908af-df0d-4457-97e2-0018669e4b9c" };

            connection.Execute(sql, @params, commandType: CommandType.StoredProcedure);
        }
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[procedure]";

            connection.Query<Category>(procedure, commandType: CommandType.StoredProcedure);
        }
    
        ///<Summary>
        /// Executa e retorna o que foi gerado pelo sql
        /// SELECT SCOPE_IDENTITY() só funciona com id IDENTITY
        /// output inserted.[Id] -> funciona para guids
        ///</Summary>
        static void ExecuteScalarCategory(SqlConnection connection)
        {
            var insert = 
            @"INSERT INTO CATEGORY 
            OUTPUT inserted.[Id]
            VALUES(NEWID(), 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert = new Category()
            {
                Title = "Amazon WS",
                Url = "Amazon",
                Description = "Serviços WS",
                Order = 8,
                Summary = "Aws CLOUD"
            };

            var id = connection.ExecuteScalar<Guid>(insert, categoryInsert);
            Console.WriteLine($"{id} gerado");

        }
        
        static void OneToOne(SqlConnection connection)
        {
            var sql = "SELECT [CareerItem].[CareerId] as Id, [CareerItem].[Title], [Course].[Id], [Course].[Title] FROM [CareerItem] INNER JOIN [Course] on [CareerItem].[CourseId] = [Course].[Id]";

            var items = connection.Query<CareerItem, Course, CareerItem>(sql, (careerItem, course) => 
            {
                careerItem.Course = course;
                return careerItem;
            }, splitOn: "Id");

            foreach (var item in items)
            {
                Console.WriteLine($"id: {item.Id} - Title: {item.Title}");
            }
        }
    
        static void OneToMany(SqlConnection connection)
        {
            var sql = "SELECT [Career].[Id], [Career].[Title], [CareerItem].[CareerId] as Id, [CareerItem].[Title] FROM [Career] INNER JOIN [CareerItem] on [Career].[Id] = [CareerItem].[CareerId]";
            
            var careers = new List<Career>();
            var items = connection.Query<Career, CareerItem, Career>(sql, (career, careerItem) => 
            {
                var car = careers.FirstOrDefault(x => x.Id == career.Id);
                if(car == null)
                {
                    car = career;
                    car.CareerItems.Add(careerItem);
                    careers.Add(car);
                }
                else
                {
                    car.CareerItems.Add(careerItem);
                }
                return career;
            }, splitOn: "Id");

            foreach (var item in careers)
            {
                Console.WriteLine($"id: {item.Id} - Title: {item.Title}");

                foreach (var carrer in item.CareerItems)
                {
                    Console.WriteLine($" - id: {carrer.Id} - Title: {carrer.Title}");
                }
            }
        }

        static void QueryMultiple(SqlConnection connection)
        {
            var query = "SELECT * FROM [Category]; SELECT * FROM [Course]; SELECT * FROM [Career]";

            using(var mult = connection.QueryMultiple(query))
            {
                var categories = mult.Read<Category>();
                var courses = mult.Read<Course>();
                var careers = mult.Read<Career>();

                foreach (var category in categories)
                {
                    Console.WriteLine($"Category - id: {category.Id} - Title: {category.Title}");
                }

                foreach (var course in courses)
                {
                    Console.WriteLine($"Course - id: {course.Id} - Title: {course.Title}");
                }

                foreach (var career in careers)
                {
                    Console.WriteLine($"Career - id: {career.Id} - Title: {career.Title}");
                }

            }
        }
    
        static void SelectIn(SqlConnection connection)
        {
            var query = "select * from Career where id in @id";

            var careers = connection.Query<Career>(query, new {
                id = new[]{ "4327ac7e-963b-4893-9f31-9a3b28a4e72b", "92d7e864-bea5-4812-80cc-c2f4e94db1af"}
            });

            foreach (var career in careers)
            {
                Console.WriteLine($"Career - id: {career.Id} - Title: {career.Title}");
            }
        }

        static void Like(SqlConnection connection, string term)
        {

            var query = "select * FROM [Course] where [Title] like @exp";

            var courses = connection.Query<Course>(query, new {
                exp = $"%{term}%"
            });

            foreach (var course in courses)
            {
                Console.WriteLine($"Course - id: {course.Id} - Title: {course.Title}");
            }
        }
    
        static void Transaction(SqlConnection connection)
        {
            connection.Open();
            var insert = 
            @"INSERT INTO CATEGORY VALUES(@Id, 
            @Title, 
            @Url, 
            @Summary, 
            @Order,  
            @Description, 
            @Featured)";

            var categoryInsert = new Category()
            {
                Id = Guid.NewGuid(),
                Title = "Amazon WS quero salvar",
                Url = "Amazon",
                Description = "Serviços WS",
                Order = 8,
                Summary = "Aws CLOUD"
            };
            using(var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insert, categoryInsert, transaction);
                transaction.Commit();
            }
        }
    }

}
